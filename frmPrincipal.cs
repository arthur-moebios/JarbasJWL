using System;
using System.Text;
using System.Windows.Forms;
using System.Security.Claims;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using ZOOM_SDK_DOTNET_WRAP;
using Microsoft.Win32;
using System.Threading;
using System.Text.RegularExpressions;
using System.Windows.Automation;
using System.Linq;
using System.ComponentModel;
using System.Web;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.VisualStudio.Services.OAuth;
using System.Text.Json;
using System.Security.Cryptography;

namespace JarbasJWL
{
    public partial class frmPrincipal : Form
    {
        private readonly string _appString = "JarbasJWL";
        private Mutex _appMutex;
        private HWNDDotNet jwLibraryWindowHandle;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmPrincipal());
        }

        public frmPrincipal()
        {
            if (AnotherInstanceRunning())
            {
                Application.Exit();
            }
            
            InitializeComponent();

            Task.Run(() =>
            {
                while (!ProcessExists(JwLibProcessName))
                {
                    System.Threading.Thread.Sleep(1000);
                }

                // Assim que o processo for encontrado, atualize a UI
                Invoke((Action)(() =>
                {
                    InitializeProcess();
                    pbDetecting.Visible = false;
                    lbDetecting.Visible = false;
                }));
            });
            InitApp();
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPrincipal_FormClosed);
        }

        private void InitializeProcess()
        {
            var jwLibraryProcess = Process.GetProcessesByName(JwLibProcessName);

            if (jwLibraryProcess != null)
            {
                var jwLibraryWindow = AutomationElement.RootElement.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, JwLibCaption));

                var conditionWebView = new AndCondition(
                   new PropertyCondition(AutomationElement.FrameworkIdProperty, "MicrosoftEdge"),
                   new PropertyCondition(AutomationElement.ClassNameProperty, "WebView"));

                AutomationElement WebViewElement = jwLibraryWindow.FindFirst(TreeScope.Descendants, conditionWebView);

                var conditionMain = new AndCondition(
                   new PropertyCondition(AutomationElement.NameProperty, JwLibCaption),
                   new PropertyCondition(AutomationElement.ClassNameProperty, "Windows.UI.Core.CoreWindow"));

                AutomationElementCollection MainWindows = AutomationElement.RootElement.FindAll(TreeScope.Descendants, conditionMain);

                foreach (AutomationElement mainWindowElement in MainWindows)
                {
                    Condition conditionFirstChild = Condition.TrueCondition;
                    AutomationElement firstChild = mainWindowElement.FindFirst(TreeScope.Children, conditionFirstChild);
                    if (firstChild != null)
                    {
                        if (firstChild.Current.Name != "imagem")
                            Automation.AddStructureChangedEventHandler(mainWindowElement, TreeScope.Descendants, new StructureChangedEventHandler(OnStructureChanged));
                    }
                    if (WebViewElement != null)
                    {
                        jwLibraryWindowHandle.value = (uint)(IntPtr)mainWindowElement.Current.NativeWindowHandle;
                    }
                }

                if (WebViewElement != null)
                {
                    TreeWalker walker = TreeWalker.ControlViewWalker;
                    AutomationElement parent = walker.GetParent(WebViewElement);

                    if (parent.Current.ClassName == "Windows.UI.Core.CoreWindow")
                    {
                        jwLibraryWindowHandle.value = (uint)(IntPtr)parent.Current.NativeWindowHandle;
                    }
                }
            }
        }

        private void frmPrincipal_FormClosed(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        private bool AnotherInstanceRunning()
        {
            _appMutex = new Mutex(true, _appString, out var newInstance);
            return !newInstance;
        }

        // Método para verificar se um processo está em execução
        static bool ProcessExists(string processName)
        {
            // Obtém todos os processos com o nome especificado
            Process[] processes = Process.GetProcessesByName(processName);
            return processes.Length > 0; // Retorna true se pelo menos um processo for encontrado
        }

        private const string JwLibProcessName = "JWLibrary";
        private const string JwLibCaption = "JW Library";

        public static BackgroundWorker worker { get; private set; }
        public Process jwLibraryProcess { get; private set; }

        // Importação da API do Windows para obter as dimensões da janela
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        private void InitApp()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "Jarbas JWLibrary";

            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Jarbas", true);
            if (key != null)
            {
                tbClientID.Text = (string)key.GetValue("CID");
                tbClientSecret.Text = (string)key.GetValue("CSecret");
                tbIDReuniao.Text = (string)key.GetValue("ID");
                tbSenha.Text = (string)key.GetValue("Senha");
                tbNome.Text = (string)key.GetValue("Nome");
                key.Close();
            }            
        }

        //onMeetingStatusChanged
        public void onMeetingStatusChanged(MeetingStatus status, int iResult)
        {
            switch (status)
            {
                case MeetingStatus.MEETING_STATUS_IDLE:
                    pbZoom.Visible = false;
                    pbDetecting.Visible = false;
                    lbDetecting.Visible = false;
                    lbDetecting.Text = "Não há uma reunião.";
                    break;
                case MeetingStatus.MEETING_STATUS_CONNECTING:
                    lbDetecting.Text = "Conectando ao servidor Zoom.";
                    break;
                case MeetingStatus.MEETING_STATUS_WAITINGFORHOST:
                    lbDetecting.Text = "Aguardando o Anfitrião iniciar a reunião.";
                    break;
                case MeetingStatus.MEETING_STATUS_INMEETING:
                    lbDetecting.Text = "Reunião em transmissão.";
                    break;
                case MeetingStatus.MEETING_STATUS_DISCONNECTING:
                    pbZoom.Visible = false;
                    pbDetecting.Visible = false;
                    lbDetecting.Visible = false;
                    lbDetecting.Text = "Desconectando do servidor.";
                    break;
                case MeetingStatus.MEETING_STATUS_RECONNECTING:
                    lbDetecting.Text = "Reconectando a reunião.";
                    break;
                case MeetingStatus.MEETING_STATUS_FAILED:
                    pbZoom.Visible = false;
                    pbDetecting.Visible = false;
                    lbDetecting.Visible = false;
                    lbDetecting.Text = "Falha ao conectar ao servidor.";
                    break;
                case MeetingStatus.MEETING_STATUS_ENDED:
                    pbZoom.Visible = false;
                    pbDetecting.Visible = false;
                    lbDetecting.Visible = false;
                    lbDetecting.Text = "Reunião encerrada.";
                    break;
                case MeetingStatus.MEETING_STATUS_UNKNOW:
                    pbZoom.Visible = false;
                    pbDetecting.Visible = false;
                    lbDetecting.Visible = false;
                    lbDetecting.Text = "Erro desconhecido.";
                    break;
                case MeetingStatus.MEETING_STATUS_LOCKED:
                    lbDetecting.Text = "A reunião está bloqueada para novas conexões.";
                    break;
                case MeetingStatus.MEETING_STATUS_UNLOCKED:
                    lbDetecting.Text = "A reunião está desbloqueada para novas conexões.";
                    break;
                case MeetingStatus.MEETING_STATUS_IN_WAITING_ROOM:
                    lbDetecting.Text = "Participantes que iniciaram antes da reunião começar estarão na sala de espera.";
                    break;
                case MeetingStatus.MEETING_STATUS_WEBINAR_PROMOTE:
                    lbDetecting.Text = "Upgrade the attendees to panelist in webinar.";
                    break;
                case MeetingStatus.MEETING_STATUS_WEBINAR_DEPROMOTE:
                    lbDetecting.Text = "Downgrade the attendees from the panelist.";
                    break;
                case MeetingStatus.MEETING_STATUS_JOIN_BREAKOUT_ROOM:
                    lbDetecting.Text = "Entrando na sala simultânea.";
                    break;
                case MeetingStatus.MEETING_STATUS_LEAVE_BREAKOUT_ROOM:
                    lbDetecting.Text = "Saindo da sala simultânea.";
                    break;
                case MeetingStatus.MEETING_STATUS_WAITING_EXTERNAL_SESSION_KEY:
                    lbDetecting.Text = "Waiting for the additional secret key.";
                    break;
                default:
                    pbZoom.Visible = false;
                    pbDetecting.Visible = false;
                    lbDetecting.Visible = false;
                    lbDetecting.Text = "Status desconhecido.";
                    break;
            }
        }

        public void onUserJoin(Array lstUserID)
        {
            if (null == (Object)lstUserID)
                return;

            for (int i = lstUserID.GetLowerBound(0); i <= lstUserID.GetUpperBound(0); i++)
            {
                UInt32 userid = (UInt32)lstUserID.GetValue(i);
                IUserInfoDotNetWrap user = CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().
                    GetMeetingParticipantsController().GetUserByUserID(userid);
                if (null != (Object)user)
                {
                    string name = user.GetUserNameW();
                    Console.Write(name);
                }
            }
        }
        public void onUserLeft(Array lstUserID)
        {
            //todo
        }
        public void onHostChangeNotification(UInt32 userId)
        {
            //todo
        }
        public void onLowOrRaiseHandStatusChanged(bool bLow, UInt32 userid)
        {
            //todo
        }
        public void onUserNamesChanged(Array lstUserID)
        {
            //todo
        }
        private void RegisterCallBack()
        {
            CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().Add_CB_onMeetingStatusChanged(onMeetingStatusChanged);
            CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().GetMeetingWaitingRoomController().Add_CB_onWaitingRoomUserJoin(onWatingRoomUserJoin);
        }

        public void onAuthenticationReturn(AuthResult ret)
        {
            pbZoom.Visible = true;
            lbDetecting.Text = "Entrando na reunião";
            lbDetecting.Visible = true;

            if (AuthResult.AUTHRET_SUCCESS == ret)
            {
                tbIDReuniao.Text = Regex.Replace(tbIDReuniao.Text, @"\D", "");

                RegisterCallBack();
                JoinParam joinparam = new JoinParam();
                joinparam.userType = SDKUserType.SDK_UT_WITHOUT_LOGIN;
                JoinParam4WithoutLogin join_withoutlogin_param = new JoinParam4WithoutLogin();
                join_withoutlogin_param.meetingNumber = UInt64.Parse(tbIDReuniao.Text);
                join_withoutlogin_param.psw = tbSenha.Text;
                join_withoutlogin_param.userName = tbNome.Text;
                joinparam.withoutloginJoin = join_withoutlogin_param;

                ZOOM_SDK_DOTNET_WRAP.SDKError joinerr = ZOOM_SDK_DOTNET_WRAP.CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().Join(joinparam);
                if (joinerr != SDKError.SDKERR_SUCCESS)
                    MessageBox.Show(joinerr.ToString());
            }
            else//error handle.todo
            {
                var resultname = (AuthResult)ret;
                MessageBox.Show(resultname.ToString());
            }
        }
        public void OnLoginRet(LOGINSTATUS ret, IAccountInfo pAccountInfo, LOGINFAILREASON reason)
        {

        }
        public void onLogout()
        {
            //todo
        }

        private void onSharingStatus(SharingStatus status, uint userId)
        {

        }

        public void onWatingRoomUserJoin(uint userID)
        {
            CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().GetMeetingWaitingRoomController().AdmitToMeeting(userID);
        }

        private void OnStructureChanged(object sender, StructureChangedEventArgs e)
        {
            AutomationElement jwRootElement = sender as AutomationElement;

            var condition2 = new AndCondition(
                    new PropertyCondition(AutomationElement.AutomationIdProperty, "VolumeButton"),
                    new PropertyCondition(AutomationElement.NameProperty, "Desativar áudio"),
                    new PropertyCondition(AutomationElement.ClassNameProperty, "Button"));

            AutomationElement IsVideo = jwRootElement.FindFirst(TreeScope.Descendants, condition2);

            if (IsVideo == null)
            {
                // Tentar encontrar o elemento de interesse
                var condition = new AndCondition(
                new PropertyCondition(AutomationElement.NameProperty, "Watchtower.Apps.JWLibrary.Common.ViewModels.PlayMedia.ImageViewModel"),
                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ListItem),
                new PropertyCondition(AutomationElement.ClassNameProperty, "FlipViewItem"));

                AutomationElement IsImage = jwRootElement.FindFirst(TreeScope.Descendants, condition);


                if (IsImage != null)
                {
                    // Elemento foi encontrado, execute sua ação aqui
                    if (CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().GetMeetingShareController().CanStartShare())
                    {
                        Action action = new Action(() =>
                        {
                            CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().GetMeetingShareController().EnableShareComputerSound(true);
                            CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().GetMeetingShareController().EnableOptimizeForFullScreenVideoClip(true);
                            CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().GetMeetingShareController().StartAppShare(jwLibraryWindowHandle);
                        });
                        Invoke(action);
                    }
                }
            }
            else
            {

                // Elemento foi encontrado, execute sua ação aqui
                if (CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().GetMeetingShareController().CanStartShare())
                {
                    System.Action action = new Action(() =>
                    {
                        CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().GetMeetingShareController().EnableShareComputerSound(true);
                        CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().GetMeetingShareController().EnableOptimizeForFullScreenVideoClip(true);
                        CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().GetMeetingShareController().StartAppShare(jwLibraryWindowHandle);
                    });
                    Invoke(action);
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            // Definir o tempo atual em UTC e o tempo de expiração (adicionando 1 dia)
            var currentTimeUtc = DateTime.UtcNow;
            var expirationTimeUtc = currentTimeUtc.AddDays(1);

            // Converter para GMT-3
            var gmtMinus3Hours = TimeSpan.FromHours(-3);
            var currentTimeGmtMinus3 = currentTimeUtc.Add(gmtMinus3Hours);
            var expirationTimeGmtMinus3 = expirationTimeUtc.Add(gmtMinus3Hours);

            // Converter para epoch time considerando o fuso GMT-3
            var epochStartTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var iat = (long)(currentTimeGmtMinus3 - epochStartTime).TotalSeconds;
            var exp = (long)(expirationTimeGmtMinus3 - epochStartTime).TotalSeconds;

            // Chave secreta para a assinatura
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tbClientSecret.Text));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Criar claims baseadas no payload fornecido
            var claims = new[]
            {
            new Claim("appKey", tbClientID.Text),
            new Claim(JwtRegisteredClaimNames.Iat, iat.ToString(), ClaimValueTypes.Integer64),
            new Claim(JwtRegisteredClaimNames.Exp, exp.ToString(), ClaimValueTypes.Integer64),
            new Claim("tokenExp", exp.ToString(), ClaimValueTypes.Integer64)
            };

            // Criar o token JWT
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: credentials
            );

            // Gerar o token
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Jarbas");
            key.SetValue("CID", tbClientID.Text);
            key.SetValue("CSecret", tbClientSecret.Text);
            key.SetValue("ID", tbIDReuniao.Text);
            key.SetValue("Senha", tbSenha.Text);
            key.SetValue("Nome", tbNome.Text);
            key.Close();

            //init sdk
            {
                InitParam param = new InitParam();
                param.web_domain = "https://zoom.us";
                param.language_id = SDK_LANGUAGE_ID.LANGUAGE_Portuguese;
                SDKError err = CZoomSDKeDotNetWrap.Instance.Initialize(param);

                if (SDKError.SDKERR_SUCCESS == err)
                {
                    //register callback
                    CZoomSDKeDotNetWrap.Instance.GetAuthServiceWrap().Add_CB_onAuthenticationReturn(onAuthenticationReturn);
                    CZoomSDKeDotNetWrap.Instance.GetAuthServiceWrap().Add_CB_onLogout(onLogout);
                    //
                    AuthContext Authparam = new AuthContext();
                    Authparam.jwt_token = jwtToken;
                    SDKError errorAuthn = CZoomSDKeDotNetWrap.Instance.GetAuthServiceWrap().SDKAuth(Authparam);

                    if (SDKError.SDKERR_SUCCESS == errorAuthn)
                    {
                        pbZoom.Visible = true;
                        lbDetecting.Text = "Autenticando no Zoom";
                        lbDetecting.Visible = true;
                    }
                    else//error handle
                    {
                        MessageBox.Show("Erro na autenticação do SDK " + errorAuthn.ToString());

                    }
                }
                else//error handle.todo
                {
                    MessageBox.Show("Erro na inicialização do SDK " + err.ToString());
                }
            }            
        }
    }
}
