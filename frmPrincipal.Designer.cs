
namespace JarbasJWL
{
    partial class frmPrincipal
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPrincipal));
            this.tbIDReuniao = new System.Windows.Forms.TextBox();
            this.tbSenha = new System.Windows.Forms.TextBox();
            this.tbNome = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.lbDetecting = new System.Windows.Forms.Label();
            this.pbZoom = new System.Windows.Forms.PictureBox();
            this.pbDetecting = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbZoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDetecting)).BeginInit();
            this.SuspendLayout();
            // 
            // tbIDReuniao
            // 
            this.tbIDReuniao.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbIDReuniao.Location = new System.Drawing.Point(29, 97);
            this.tbIDReuniao.Name = "tbIDReuniao";
            this.tbIDReuniao.Size = new System.Drawing.Size(361, 26);
            this.tbIDReuniao.TabIndex = 0;
            // 
            // tbSenha
            // 
            this.tbSenha.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbSenha.Location = new System.Drawing.Point(29, 158);
            this.tbSenha.Name = "tbSenha";
            this.tbSenha.Size = new System.Drawing.Size(361, 26);
            this.tbSenha.TabIndex = 1;
            // 
            // tbNome
            // 
            this.tbNome.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbNome.Location = new System.Drawing.Point(29, 224);
            this.tbNome.Name = "tbNome";
            this.tbNome.Size = new System.Drawing.Size(361, 26);
            this.tbNome.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(25, 135);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Senha";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(26, 201);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(144, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "Nome de usuário";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(30, 267);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(360, 35);
            this.button1.TabIndex = 6;
            this.button1.Text = "Entrar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
            this.pictureBox1.Location = new System.Drawing.Point(396, 21);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(108, 115);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox2.InitialImage")));
            this.pictureBox2.Location = new System.Drawing.Point(396, 190);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(108, 112);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 8;
            this.pictureBox2.TabStop = false;
            // 
            // lbDetecting
            // 
            this.lbDetecting.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbDetecting.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDetecting.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lbDetecting.Location = new System.Drawing.Point(0, 0);
            this.lbDetecting.Name = "lbDetecting";
            this.lbDetecting.Size = new System.Drawing.Size(516, 56);
            this.lbDetecting.TabIndex = 14;
            this.lbDetecting.Text = "Detectando JW Library";
            this.lbDetecting.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbZoom
            // 
            this.pbZoom.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbZoom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbZoom.Image = ((System.Drawing.Image)(resources.GetObject("pbZoom.Image")));
            this.pbZoom.Location = new System.Drawing.Point(0, 56);
            this.pbZoom.Name = "pbZoom";
            this.pbZoom.Size = new System.Drawing.Size(516, 356);
            this.pbZoom.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbZoom.TabIndex = 16;
            this.pbZoom.TabStop = false;
            this.pbZoom.UseWaitCursor = true;
            this.pbZoom.Visible = false;
            // 
            // pbDetecting
            // 
            this.pbDetecting.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbDetecting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbDetecting.Image = ((System.Drawing.Image)(resources.GetObject("pbDetecting.Image")));
            this.pbDetecting.Location = new System.Drawing.Point(0, 56);
            this.pbDetecting.Name = "pbDetecting";
            this.pbDetecting.Size = new System.Drawing.Size(516, 356);
            this.pbDetecting.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbDetecting.TabIndex = 17;
            this.pbDetecting.TabStop = false;
            this.pbDetecting.UseWaitCursor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(26, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "ID da Reunião";
            // 
            // frmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(516, 412);
            this.Controls.Add(this.pbZoom);
            this.Controls.Add(this.pbDetecting);
            this.Controls.Add(this.lbDetecting);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbIDReuniao);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbSenha);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbNome);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmPrincipal";
            this.Text = "Jarbas JWLibrary Integration";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbZoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDetecting)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbIDReuniao;
        private System.Windows.Forms.TextBox tbSenha;
        private System.Windows.Forms.TextBox tbNome;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label lbDetecting;
        private System.Windows.Forms.PictureBox pbZoom;
        private System.Windows.Forms.PictureBox pbDetecting;
        private System.Windows.Forms.Label label1;
    }
}

