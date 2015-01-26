<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Login
    Inherits System.Windows.Forms.Form

    'Form esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Richiesto da Progettazione Windows Form
    Private components As System.ComponentModel.IContainer

    'NOTA: la procedura che segue è richiesta da Progettazione Windows Form
    'Può essere modificata in Progettazione Windows Form.  
    'Non modificarla nell'editor del codice.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Button_Cancel = New System.Windows.Forms.Button
        Me.TextBox_Level_Required = New System.Windows.Forms.TextBox
        Me.Label_Livello_Richiesto = New System.Windows.Forms.Label
        Me.Button_Login = New System.Windows.Forms.Button
        Me.TextBox_Password = New System.Windows.Forms.TextBox
        Me.TextBox_Utente = New System.Windows.Forms.TextBox
        Me.Label_Password = New System.Windows.Forms.Label
        Me.Label_Utente = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'Button_Cancel
        '
        Me.Button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button_Cancel.Location = New System.Drawing.Point(180, 204)
        Me.Button_Cancel.Name = "Button_Cancel"
        Me.Button_Cancel.Size = New System.Drawing.Size(100, 50)
        Me.Button_Cancel.TabIndex = 23
        Me.Button_Cancel.Text = "Cancel"
        Me.Button_Cancel.UseVisualStyleBackColor = True
        '
        'TextBox_Level_Required
        '
        Me.TextBox_Level_Required.Location = New System.Drawing.Point(15, 131)
        Me.TextBox_Level_Required.Name = "TextBox_Level_Required"
        Me.TextBox_Level_Required.ReadOnly = True
        Me.TextBox_Level_Required.Size = New System.Drawing.Size(100, 20)
        Me.TextBox_Level_Required.TabIndex = 21
        Me.TextBox_Level_Required.TabStop = False
        '
        'Label_Livello_Richiesto
        '
        Me.Label_Livello_Richiesto.AutoSize = True
        Me.Label_Livello_Richiesto.Location = New System.Drawing.Point(12, 115)
        Me.Label_Livello_Richiesto.Name = "Label_Livello_Richiesto"
        Me.Label_Livello_Richiesto.Size = New System.Drawing.Size(84, 13)
        Me.Label_Livello_Richiesto.TabIndex = 26
        Me.Label_Livello_Richiesto.Text = "Livello Richiesto"
        '
        'Button_Login
        '
        Me.Button_Login.Location = New System.Drawing.Point(12, 204)
        Me.Button_Login.Name = "Button_Login"
        Me.Button_Login.Size = New System.Drawing.Size(100, 50)
        Me.Button_Login.TabIndex = 22
        Me.Button_Login.Text = "Login"
        Me.Button_Login.UseVisualStyleBackColor = True
        '
        'TextBox_Password
        '
        Me.TextBox_Password.Location = New System.Drawing.Point(15, 79)
        Me.TextBox_Password.Name = "TextBox_Password"
        Me.TextBox_Password.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.TextBox_Password.Size = New System.Drawing.Size(100, 20)
        Me.TextBox_Password.TabIndex = 20
        '
        'TextBox_Utente
        '
        Me.TextBox_Utente.Location = New System.Drawing.Point(15, 29)
        Me.TextBox_Utente.Name = "TextBox_Utente"
        Me.TextBox_Utente.Size = New System.Drawing.Size(100, 20)
        Me.TextBox_Utente.TabIndex = 19
        '
        'Label_Password
        '
        Me.Label_Password.AutoSize = True
        Me.Label_Password.Location = New System.Drawing.Point(12, 63)
        Me.Label_Password.Name = "Label_Password"
        Me.Label_Password.Size = New System.Drawing.Size(53, 13)
        Me.Label_Password.TabIndex = 25
        Me.Label_Password.Text = "Password"
        '
        'Label_Utente
        '
        Me.Label_Utente.AutoSize = True
        Me.Label_Utente.Location = New System.Drawing.Point(12, 13)
        Me.Label_Utente.Name = "Label_Utente"
        Me.Label_Utente.Size = New System.Drawing.Size(39, 13)
        Me.Label_Utente.TabIndex = 24
        Me.Label_Utente.Text = "Utente"
        '
        'Login
        '
        Me.AcceptButton = Me.Button_Login
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.CancelButton = Me.Button_Cancel
        Me.ClientSize = New System.Drawing.Size(292, 266)
        Me.Controls.Add(Me.Button_Cancel)
        Me.Controls.Add(Me.TextBox_Level_Required)
        Me.Controls.Add(Me.Label_Livello_Richiesto)
        Me.Controls.Add(Me.Button_Login)
        Me.Controls.Add(Me.TextBox_Password)
        Me.Controls.Add(Me.TextBox_Utente)
        Me.Controls.Add(Me.Label_Password)
        Me.Controls.Add(Me.Label_Utente)
        Me.DoubleBuffered = True
        Me.Name = "Login"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button_Cancel As System.Windows.Forms.Button
    Friend WithEvents TextBox_Level_Required As System.Windows.Forms.TextBox
    Friend WithEvents Label_Livello_Richiesto As System.Windows.Forms.Label
    Friend WithEvents Button_Login As System.Windows.Forms.Button
    Friend WithEvents TextBox_Password As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_Utente As System.Windows.Forms.TextBox
    Friend WithEvents Label_Password As System.Windows.Forms.Label
    Friend WithEvents Label_Utente As System.Windows.Forms.Label
End Class
