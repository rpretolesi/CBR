<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class GestNomiPreDos
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
        Me.Label_NPPD_Tramoggia = New System.Windows.Forms.Label
        Me.Label_NPPD_Nome_Prodotto = New System.Windows.Forms.Label
        Me.Label_NPPD_Umidita_Perc = New System.Windows.Forms.Label
        Me.TextBox_NPPD_Umidita_Perc = New System.Windows.Forms.TextBox
        Me.TextBox_NPPD_Nome_Prodotto = New System.Windows.Forms.TextBox
        Me.TextBox_NPPD_Tramoggia = New System.Windows.Forms.TextBox
        CType(Me.m_ds, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.m_bs, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label_NPPD_Tramoggia
        '
        Me.Label_NPPD_Tramoggia.AutoSize = True
        Me.Label_NPPD_Tramoggia.Location = New System.Drawing.Point(12, 55)
        Me.Label_NPPD_Tramoggia.Name = "Label_NPPD_Tramoggia"
        Me.Label_NPPD_Tramoggia.Size = New System.Drawing.Size(125, 13)
        Me.Label_NPPD_Tramoggia.TabIndex = 31
        Me.Label_NPPD_Tramoggia.Text = "Label_NPPD_Tramoggia"
        '
        'Label_NPPD_Nome_Prodotto
        '
        Me.Label_NPPD_Nome_Prodotto.AutoSize = True
        Me.Label_NPPD_Nome_Prodotto.Location = New System.Drawing.Point(12, 81)
        Me.Label_NPPD_Nome_Prodotto.Name = "Label_NPPD_Nome_Prodotto"
        Me.Label_NPPD_Nome_Prodotto.Size = New System.Drawing.Size(149, 13)
        Me.Label_NPPD_Nome_Prodotto.TabIndex = 32
        Me.Label_NPPD_Nome_Prodotto.Text = "Label_NPPD_Nome_Prodotto"
        '
        'Label_NPPD_Umidita_Perc
        '
        Me.Label_NPPD_Umidita_Perc.AutoSize = True
        Me.Label_NPPD_Umidita_Perc.Location = New System.Drawing.Point(12, 107)
        Me.Label_NPPD_Umidita_Perc.Name = "Label_NPPD_Umidita_Perc"
        Me.Label_NPPD_Umidita_Perc.Size = New System.Drawing.Size(138, 13)
        Me.Label_NPPD_Umidita_Perc.TabIndex = 33
        Me.Label_NPPD_Umidita_Perc.Text = "Label_NPPD_Umidita_Perc"
        '
        'TextBox_NPPD_Umidita_Perc
        '
        Me.TextBox_NPPD_Umidita_Perc.Location = New System.Drawing.Point(193, 104)
        Me.TextBox_NPPD_Umidita_Perc.Name = "TextBox_NPPD_Umidita_Perc"
        Me.TextBox_NPPD_Umidita_Perc.Size = New System.Drawing.Size(100, 20)
        Me.TextBox_NPPD_Umidita_Perc.TabIndex = 34
        '
        'TextBox_NPPD_Nome_Prodotto
        '
        Me.TextBox_NPPD_Nome_Prodotto.Location = New System.Drawing.Point(193, 78)
        Me.TextBox_NPPD_Nome_Prodotto.Name = "TextBox_NPPD_Nome_Prodotto"
        Me.TextBox_NPPD_Nome_Prodotto.ReadOnly = True
        Me.TextBox_NPPD_Nome_Prodotto.Size = New System.Drawing.Size(100, 20)
        Me.TextBox_NPPD_Nome_Prodotto.TabIndex = 35
        '
        'TextBox_NPPD_Tramoggia
        '
        Me.TextBox_NPPD_Tramoggia.Location = New System.Drawing.Point(193, 52)
        Me.TextBox_NPPD_Tramoggia.Name = "TextBox_NPPD_Tramoggia"
        Me.TextBox_NPPD_Tramoggia.ReadOnly = True
        Me.TextBox_NPPD_Tramoggia.Size = New System.Drawing.Size(100, 20)
        Me.TextBox_NPPD_Tramoggia.TabIndex = 36
        '
        'GestNomiPreDos
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.ClientSize = New System.Drawing.Size(792, 566)
        Me.Controls.Add(Me.TextBox_NPPD_Tramoggia)
        Me.Controls.Add(Me.TextBox_NPPD_Nome_Prodotto)
        Me.Controls.Add(Me.Label_NPPD_Tramoggia)
        Me.Controls.Add(Me.Label_NPPD_Nome_Prodotto)
        Me.Controls.Add(Me.Label_NPPD_Umidita_Perc)
        Me.Controls.Add(Me.TextBox_NPPD_Umidita_Perc)
        Me.Name = "GestNomiPreDos"
        Me.Controls.SetChildIndex(Me.TextBox_NPPD_Umidita_Perc, 0)
        Me.Controls.SetChildIndex(Me.Label_NPPD_Umidita_Perc, 0)
        Me.Controls.SetChildIndex(Me.Label_NPPD_Nome_Prodotto, 0)
        Me.Controls.SetChildIndex(Me.Label_NPPD_Tramoggia, 0)
        Me.Controls.SetChildIndex(Me.TextBox_NPPD_Nome_Prodotto, 0)
        Me.Controls.SetChildIndex(Me.TextBox_NPPD_Tramoggia, 0)
        CType(Me.m_ds, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.m_bs, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label_NPPD_Tramoggia As System.Windows.Forms.Label
    Friend WithEvents Label_NPPD_Nome_Prodotto As System.Windows.Forms.Label
    Friend WithEvents Label_NPPD_Umidita_Perc As System.Windows.Forms.Label
    Friend WithEvents TextBox_NPPD_Umidita_Perc As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_NPPD_Nome_Prodotto As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_NPPD_Tramoggia As System.Windows.Forms.TextBox

End Class
