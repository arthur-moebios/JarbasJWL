using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Claims;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using ZOOM_SDK_DOTNET_WRAP;
using System.Windows.Automation;

namespace JarbasJWL
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitApp();
        }

         private const string JwLibProcessName = "JWLibrary";
        private const string JwLibCaption = "JW Library";
        private bool AuthenticatedZoom = false;

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
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("4PBb61bRe7nXi8maNzZyenvOguudWo37"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Criar claims baseadas no payload fornecido
            var claims = new[]
            {
            new Claim("appKey", "8EqLCV_7Q8GQVYKM5SC96g"),
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
                    CZoomSDKeDotNetWrap.Instance.GetAuthServiceWrap().Add_CB_onLoginRet(onLoginRet);
                    CZoomSDKeDotNetWrap.Instance.GetAuthServiceWrap().Add_CB_onLogout(onLogout);
                    //
                    AuthContext Authparam = new AuthContext();
                    Authparam.jwt_token = jwtToken;
                    SDKError errorAuthn = CZoomSDKeDotNetWrap.Instance.GetAuthServiceWrap().SDKAuth(Authparam);

                    if (SDKError.SDKERR_SUCCESS == errorAuthn)
                    {

                    }
                    else//error handle
                    {
                        MessageBox.Show("Erro na autenticação do SDK");

                    }
                }
                else//error handle.todo
                {
                    MessageBox.Show("Erro na inicialização do SDK " + err.ToString());
                }
            }
        }

        //onMeetingStatusChanged
        public void onMeetingStatusChanged(MeetingStatus status, int iResult)
        {
            switch (status)
            {
                case MeetingStatus.MEETING_STATUS_ENDED:
                    {
                        MessageBox.Show("A Reunião foi encerrada");
                    }
                    break;
                case MeetingStatus.MEETING_STATUS_FAILED:
                    {
                        MessageBox.Show("A Reunião falhou");
                    }
                    break;
                default://todo
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
            CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().GetMeetingParticipantsController().Add_CB_onHostChangeNotification(onHostChangeNotification);
            CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().GetMeetingParticipantsController().Add_CB_onLowOrRaiseHandStatusChanged(onLowOrRaiseHandStatusChanged);
            CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().GetMeetingParticipantsController().Add_CB_onUserJoin(onUserJoin);
            CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().GetMeetingParticipantsController().Add_CB_onUserLeft(onUserLeft);
            CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().GetMeetingParticipantsController().Add_CB_onUserNamesChanged(onUserNamesChanged);
            CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().GetMeetingWaitingRoomController().Add_CB_onWaitingRoomUserJoin(onWatingRoomUserJoin);
        }

        public void onAuthenticationReturn(AuthResult ret)
        {
            if (AuthResult.AUTHRET_SUCCESS == ret)
            {
                RegisterCallBack();
                AuthenticatedZoom = true;
            }
            else//error handle.todo
            {
                var resultname = (AuthResult)ret;
                MessageBox.Show(resultname.ToString());
            }
        }
        public void onLoginRet(LOGINSTATUS ret, IAccountInfo pAccountInfo, LOGINFAILREASON reason)
        {
            //todo
        }
        public void onLogout()
        {
            //todo
        }

        void OnStructureChanged(object sender, StructureChangedEventArgs e)
        {
            // Obter o elemento raiz (neste caso, a janela do JWLibrary)
            AutomationElement rootElement = sender as AutomationElement;

            if (rootElement == null)
            {
                return;
            }

            var condition2 = new AndCondition(
                    new PropertyCondition(AutomationElement.AutomationIdProperty, "VolumeButton"),
                    new PropertyCondition(AutomationElement.NameProperty, "Desativar áudio"),
                    new PropertyCondition(AutomationElement.ClassNameProperty, "Button"));

            AutomationElement IsVideo = rootElement.FindFirst(TreeScope.Descendants, condition2);

            if (IsVideo == null)
            {
                // Tentar encontrar o elemento de interesse
                var condition = new AndCondition(
                new PropertyCondition(AutomationElement.NameProperty, "Watchtower.Apps.JWLibrary.Common.ViewModels.PlayMedia.ImageViewModel"),
                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ListItem),
                new PropertyCondition(AutomationElement.ClassNameProperty, "FlipViewItem"));

                AutomationElement IsImage = rootElement.FindFirst(TreeScope.Descendants, condition);


                if (IsImage != null)
                {
                    // Elemento foi encontrado, execute sua ação aqui
                    if (CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().GetMeetingShareController().CanStartShare())
                    {
                        IntPtr? mediaWindowHandle = FindMediaWindowHandle();

                        // Obtém as dimensões da janela
                        GetWindowRect(mediaWindowHandle.Value, out RECT rect);

                        // Verifica em qual monitor a janela está localizada
                        Screen screen = Screen.FromHandle(mediaWindowHandle.Value);

                        Console.WriteLine($"A janela está no monitor: {screen.DeviceName}");
                        string Monitor2 = "";

                        // Para encontrar o "outro monitor", podemos iterar sobre todos os monitores e encontrar aquele que não é o atual
                        foreach (var monitor in Screen.AllScreens)
                        {
                            if (monitor.DeviceName != screen.DeviceName)
                            {
                                Monitor2 = monitor.DeviceName;
                                break; // Assumindo que há apenas dois monitores
                            }
                        }
                        Action action = new Action(() =>
                        {
                            CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().GetMeetingShareController().EnableShareComputerSound(true);
                            CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().GetMeetingShareController().EnableOptimizeForFullScreenVideoClip(true);
                            HWNDDotNet hwndSharedApp = default(HWNDDotNet);
                            hwndSharedApp.value = checked((uint)(int)mediaWindowHandle.Value);
                            CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().GetMeetingShareController().StartMonitorShare(Monitor2);
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
                    IntPtr? mediaWindowHandle = FindMediaWindowHandle();

                    // Obtém as dimensões da janela
                    GetWindowRect(mediaWindowHandle.Value, out RECT rect);

                    // Verifica em qual monitor a janela está localizada
                    Screen screen = Screen.FromHandle(mediaWindowHandle.Value);

                    Console.WriteLine($"A janela está no monitor: {screen.DeviceName}");
                    string Monitor2 = "";

                    // Para encontrar o "outro monitor", podemos iterar sobre todos os monitores e encontrar aquele que não é o atual
                    foreach (var monitor in Screen.AllScreens)
                    {
                        if (monitor.DeviceName != screen.DeviceName)
                        {
                            Monitor2 = monitor.DeviceName;
                            break; // Assumindo que há apenas dois monitores
                        }
                    }
                    System.Action action = new Action(() =>
                    {
                        CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().GetMeetingShareController().EnableShareComputerSound(true);
                        CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().GetMeetingShareController().EnableOptimizeForFullScreenVideoClip(true);
                        HWNDDotNet hwndSharedApp = default(HWNDDotNet);
                        hwndSharedApp.value = checked((uint)(int)mediaWindowHandle.Value);
                        CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().GetMeetingShareController().StartMonitorShare(Monitor2);
                    });
                    Invoke(action);

                }
            }
        }

        public void onWatingRoomUserJoin(uint userID)
        {
            CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().GetMeetingWaitingRoomController().AdmitToMeeting(userID);
        }

        // Método simplificado para retornar o handle da janela de mídia
        public IntPtr? FindMediaWindowHandle()
        {
            var jwLibraryProcess = Process.GetProcessesByName(JwLibProcessName).FirstOrDefault();
            var jwLibraryWindow = AutomationElement.RootElement.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, JwLibCaption));

            if (jwLibraryWindow != null)
            {
                IntPtr hwnd = new IntPtr(jwLibraryWindow.Current.NativeWindowHandle);
                return hwnd;
            }

            Console.WriteLine("Janela de mídia não encontrada.");
            return null;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (AuthenticatedZoom)
            {
                JoinParam paramEntrar = new JoinParam();
                paramEntrar.userType = SDKUserType.SDK_UT_WITHOUT_LOGIN;
                JoinParam4WithoutLogin join_api_param = new JoinParam4WithoutLogin();
                join_api_param.meetingNumber = UInt64.Parse(tbIDReuniao.Text);
                join_api_param.userName = tbNome.Text;
                join_api_param.psw = tbSenha.Text;
                paramEntrar.withoutloginJoin = join_api_param;

                SDKError errorJoin = CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().Join(paramEntrar);
                if (SDKError.SDKERR_SUCCESS == errorJoin)
                {
                    var jwLibraryProcess = Process.GetProcessesByName(JwLibProcessName).FirstOrDefault();
                    var jwLibraryWindow = AutomationElement.RootElement.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, JwLibCaption));

                    if (jwLibraryWindow != null)
                    {
                        Automation.AddStructureChangedEventHandler(jwLibraryWindow, TreeScope.Descendants, new StructureChangedEventHandler(OnStructureChanged));
                    }

                }
                else//error handle
                {
                    MessageBox.Show("Erro ao entrar na Reunião: " + errorJoin.ToString());
                }
            }
        }
    }
}
