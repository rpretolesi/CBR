<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class GestNomiDos
    Inherits Cybertr500.BaseGrigliaConfig

    'Form esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Richiesto da Progettazione Windows Form
    Private components As System.ComponentModel.IContainer

    'NOTA: la procedura che segue è richiesta da Progettazione Windows Form
    'Può essere modificata in Progettazione Windows Form.  
    'Non modificarla nell'editor del codice.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Label_NPD_Riferimento = New System.Windows.Forms.Label
        Me.Label_NPD_Nome = New System.Windows.Forms.Label
        Me.TextBox_NPD_Riferimento = New System.Windows.Forms.TextBox
        Me.TextBox_NPD_Nome = New System.Windows.Forms.TextBox
        CType(Me.m_ds, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.m_bs, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label_NPD_Riferimento
        '
        Me.Label_NPD_Riferimento.AutoSize = True
        Me.Label_NPD_Riferimento.Location = New System.Drawing.Point(12, 57)
        Me.Label_NPD_Riferimento.Name = "Label_NPD_Riferimento"
        Me.Label_NPD_Riferimento.Size = New System.Drawing.Size(121, 13)
        Me.Label_NPD_Riferimento.TabIndex = 30
        Me.Label_NPD_Riferimento.Text = "Label_NPD_Riferimento"
        '
        'Label_NPD_Nome
        '
        Me.Label_NPD_Nome.AutoSize = True
        Me.Label_NPD_Nome.Location = New System.Drawing.Point(12, 85)
        Me.Label_NPD_Nome.Name = "Label_NPD_Nome"
        Me.Label_NPD_Nome.Size = New System.Drawing.Size(33, 13)
        Me.Label_NPD_Nome.TabIndex = 31
        Me.Label_NPD_Nome.Text = "Label"
        '
        'TextBox_NPD_Riferimento
        '
        Me.TextBox_NPD_Riferimento.Location = New System.Drawing.Point(139, 54)
        Me.TextBox_NPD_Riferimento.Name = "TextBox_NPD_Riferimento"
        Me.TextBox_NPD_Riferimento.ReadOnly = True
        Me.TextBox_NPD_Riferimento.Size = New System.Drawing.Size(100, 20)
        Me.TextBox_NPD_Riferimento.TabIndex = 32
        '
        'TextBox_NPD_Nome
        '
        Me.TextBox_NPD_Nome.Location = New System.Drawing.Point(139, 82)
        Me.TextBox_NPD_Nome.Name = "TextBox_NPD_Nome"
        Me.TextBox_NPD_Nome.Size = New System.Drawing.Size(100, 20)
        Me.TextBox_NPD_Nome.TabIndex = 33
        '
        'GestNomiDos
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.ClientSize = New System.Drawing.Size(792, 566)
        Me.Controls.Add(Me.TextBox_NPD_Nome)
        Me.Controls.Add(Me.TextBox_NPD_Riferimento)
        Me.Controls.Add(Me.Label_NPD_Nome)
        Me.Controls.Add(Me.Label_NPD_Riferimento)
        Me.Name = "GestNomiDos"
        Me.Controls.SetChildIndex(Me.Label_NPD_Riferimento, 0)
        Me.Controls.SetChildIndex(Me.Label_NPD_Nome, 0)
        Me.Controls.SetChildIndex(Me.TextBox_NPD_Riferimento, 0)
        Me.Controls.SetChildIndex(Me.TextBox_NPD_Nome, 0)
        CType(Me.m_ds, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.m_bs, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label_NPD_Riferimento As System.Windows.Forms.Label
    Friend WithEvents Label_NPD_Nome As System.Windows.Forms.Label
    Friend WithEvents TextBox_NPD_Riferimento As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_NPD_Nome As System.Windows.Forms.TextBox

End Class
