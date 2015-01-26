<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class StatoElab
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
        Me.ProgressBar_1 = New System.Windows.Forms.ProgressBar
        Me.Button_Annulla = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'ProgressBar_1
        '
        Me.ProgressBar_1.Location = New System.Drawing.Point(12, 12)
        Me.ProgressBar_1.Name = "ProgressBar_1"
        Me.ProgressBar_1.Size = New System.Drawing.Size(268, 23)
        Me.ProgressBar_1.TabIndex = 0
        '
        'Button_Annulla
        '
        Me.Button_Annulla.Location = New System.Drawing.Point(93, 54)
        Me.Button_Annulla.Name = "Button_Annulla"
        Me.Button_Annulla.Size = New System.Drawing.Size(100, 25)
        Me.Button_Annulla.TabIndex = 1
        Me.Button_Annulla.Text = "Annulla"
        Me.Button_Annulla.UseVisualStyleBackColor = True
        '
        'StatoElab
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(292, 101)
        Me.ControlBox = False
        Me.Controls.Add(Me.Button_Annulla)
        Me.Controls.Add(Me.ProgressBar_1)
        Me.Name = "StatoElab"
        Me.Text = "StatoElab"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ProgressBar_1 As System.Windows.Forms.ProgressBar
    Friend WithEvents Button_Annulla As System.Windows.Forms.Button
End Class
