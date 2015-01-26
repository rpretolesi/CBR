<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BaseGriglia
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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(BaseGriglia))
        Me.DateTimePicker_Fine = New System.Windows.Forms.DateTimePicker
        Me.Label_DataFine = New System.Windows.Forms.Label
        Me.Label_DataInizio = New System.Windows.Forms.Label
        Me.DataGridView = New System.Windows.Forms.DataGridView
        Me.ToolStrip = New System.Windows.Forms.ToolStrip
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator
        Me.ToolStripButton_Nuovo = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.ToolStripButton_Elimina = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.ToolStripButton_Modifica = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator
        Me.ToolStripButton_Salva = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator
        Me.ToolStripButton_Annulla = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator
        Me.ToolStripButton_Stampa = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator
        Me.ToolStripButtonSelezionaTutto = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator9 = New System.Windows.Forms.ToolStripSeparator
        Me.Label_Nr_Record = New System.Windows.Forms.Label
        Me.TextBox_Nr_Record = New System.Windows.Forms.TextBox
        Me.StampaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ContextMenuStrip_1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.DateTimePicker_Inizio = New System.Windows.Forms.DateTimePicker
        CType(Me.DataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolStrip.SuspendLayout()
        Me.ContextMenuStrip_1.SuspendLayout()
        Me.SuspendLayout()
        '
        'DateTimePicker_Fine
        '
        Me.DateTimePicker_Fine.Checked = False
        Me.DateTimePicker_Fine.CustomFormat = "dddd dd/MM/yyyy hh.mm.ss"
        Me.DateTimePicker_Fine.Location = New System.Drawing.Point(399, 534)
        Me.DateTimePicker_Fine.Name = "DateTimePicker_Fine"
        Me.DateTimePicker_Fine.ShowUpDown = True
        Me.DateTimePicker_Fine.Size = New System.Drawing.Size(189, 20)
        Me.DateTimePicker_Fine.TabIndex = 18
        '
        'Label_DataFine
        '
        Me.Label_DataFine.AutoSize = True
        Me.Label_DataFine.Location = New System.Drawing.Point(311, 538)
        Me.Label_DataFine.Name = "Label_DataFine"
        Me.Label_DataFine.Size = New System.Drawing.Size(82, 13)
        Me.Label_DataFine.TabIndex = 16
        Me.Label_DataFine.Text = "Data e Ora Fine"
        '
        'Label_DataInizio
        '
        Me.Label_DataInizio.AutoSize = True
        Me.Label_DataInizio.Location = New System.Drawing.Point(12, 538)
        Me.Label_DataInizio.Name = "Label_DataInizio"
        Me.Label_DataInizio.Size = New System.Drawing.Size(86, 13)
        Me.Label_DataInizio.TabIndex = 15
        Me.Label_DataInizio.Text = "Data e Ora Inizio"
        '
        'DataGridView
        '
        Me.DataGridView.AllowUserToAddRows = False
        Me.DataGridView.AllowUserToDeleteRows = False
        Me.DataGridView.AllowUserToOrderColumns = True
        Me.DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView.Location = New System.Drawing.Point(12, 28)
        Me.DataGridView.Name = "DataGridView"
        Me.DataGridView.ReadOnly = True
        Me.DataGridView.Size = New System.Drawing.Size(768, 490)
        Me.DataGridView.TabIndex = 14
        '
        'ToolStrip
        '
        Me.ToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripSeparator5, Me.ToolStripButton_Nuovo, Me.ToolStripSeparator1, Me.ToolStripButton_Elimina, Me.ToolStripSeparator2, Me.ToolStripButton_Modifica, Me.ToolStripSeparator4, Me.ToolStripButton_Salva, Me.ToolStripSeparator3, Me.ToolStripButton_Annulla, Me.ToolStripSeparator6, Me.ToolStripButton_Stampa, Me.ToolStripSeparator8, Me.ToolStripButtonSelezionaTutto, Me.ToolStripSeparator9})
        Me.ToolStrip.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip.Name = "ToolStrip"
        Me.ToolStrip.Size = New System.Drawing.Size(792, 25)
        Me.ToolStrip.TabIndex = 27
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(6, 25)
        '
        'ToolStripButton_Nuovo
        '
        Me.ToolStripButton_Nuovo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton_Nuovo.Image = CType(resources.GetObject("ToolStripButton_Nuovo.Image"), System.Drawing.Image)
        Me.ToolStripButton_Nuovo.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton_Nuovo.Name = "ToolStripButton_Nuovo"
        Me.ToolStripButton_Nuovo.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton_Nuovo.ToolTipText = "Aggiungi"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'ToolStripButton_Elimina
        '
        Me.ToolStripButton_Elimina.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton_Elimina.Image = CType(resources.GetObject("ToolStripButton_Elimina.Image"), System.Drawing.Image)
        Me.ToolStripButton_Elimina.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton_Elimina.Name = "ToolStripButton_Elimina"
        Me.ToolStripButton_Elimina.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton_Elimina.ToolTipText = "Elimina"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 25)
        '
        'ToolStripButton_Modifica
        '
        Me.ToolStripButton_Modifica.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton_Modifica.Image = CType(resources.GetObject("ToolStripButton_Modifica.Image"), System.Drawing.Image)
        Me.ToolStripButton_Modifica.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton_Modifica.Name = "ToolStripButton_Modifica"
        Me.ToolStripButton_Modifica.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton_Modifica.Text = "ToolStripButton1"
        Me.ToolStripButton_Modifica.ToolTipText = "Modifica"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(6, 25)
        '
        'ToolStripButton_Salva
        '
        Me.ToolStripButton_Salva.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton_Salva.Image = CType(resources.GetObject("ToolStripButton_Salva.Image"), System.Drawing.Image)
        Me.ToolStripButton_Salva.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton_Salva.Name = "ToolStripButton_Salva"
        Me.ToolStripButton_Salva.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton_Salva.ToolTipText = "Salva"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(6, 25)
        '
        'ToolStripButton_Annulla
        '
        Me.ToolStripButton_Annulla.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton_Annulla.Image = CType(resources.GetObject("ToolStripButton_Annulla.Image"), System.Drawing.Image)
        Me.ToolStripButton_Annulla.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton_Annulla.Name = "ToolStripButton_Annulla"
        Me.ToolStripButton_Annulla.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton_Annulla.ToolTipText = "Annulla"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(6, 25)
        '
        'ToolStripButton_Stampa
        '
        Me.ToolStripButton_Stampa.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton_Stampa.Image = CType(resources.GetObject("ToolStripButton_Stampa.Image"), System.Drawing.Image)
        Me.ToolStripButton_Stampa.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton_Stampa.Name = "ToolStripButton_Stampa"
        Me.ToolStripButton_Stampa.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton_Stampa.ToolTipText = "Stampa"
        '
        'ToolStripSeparator8
        '
        Me.ToolStripSeparator8.Name = "ToolStripSeparator8"
        Me.ToolStripSeparator8.Size = New System.Drawing.Size(6, 25)
        '
        'ToolStripButtonSelezionaTutto
        '
        Me.ToolStripButtonSelezionaTutto.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButtonSelezionaTutto.Image = CType(resources.GetObject("ToolStripButtonSelezionaTutto.Image"), System.Drawing.Image)
        Me.ToolStripButtonSelezionaTutto.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButtonSelezionaTutto.Name = "ToolStripButtonSelezionaTutto"
        Me.ToolStripButtonSelezionaTutto.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButtonSelezionaTutto.Text = "ToolStripButtonSelezionaTutto"
        Me.ToolStripButtonSelezionaTutto.ToolTipText = "Seleziona Tutto"
        '
        'ToolStripSeparator9
        '
        Me.ToolStripSeparator9.Name = "ToolStripSeparator9"
        Me.ToolStripSeparator9.Size = New System.Drawing.Size(6, 25)
        '
        'Label_Nr_Record
        '
        Me.Label_Nr_Record.AutoSize = True
        Me.Label_Nr_Record.Location = New System.Drawing.Point(609, 538)
        Me.Label_Nr_Record.Name = "Label_Nr_Record"
        Me.Label_Nr_Record.Size = New System.Drawing.Size(59, 13)
        Me.Label_Nr_Record.TabIndex = 28
        Me.Label_Nr_Record.Text = "Nr Record:"
        '
        'TextBox_Nr_Record
        '
        Me.TextBox_Nr_Record.Location = New System.Drawing.Point(674, 535)
        Me.TextBox_Nr_Record.Name = "TextBox_Nr_Record"
        Me.TextBox_Nr_Record.ReadOnly = True
        Me.TextBox_Nr_Record.Size = New System.Drawing.Size(100, 20)
        Me.TextBox_Nr_Record.TabIndex = 29
        '
        'StampaToolStripMenuItem
        '
        Me.StampaToolStripMenuItem.Name = "StampaToolStripMenuItem"
        Me.StampaToolStripMenuItem.Size = New System.Drawing.Size(121, 22)
        Me.StampaToolStripMenuItem.Text = "Stampa"
        '
        'ContextMenuStrip_1
        '
        Me.ContextMenuStrip_1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StampaToolStripMenuItem})
        Me.ContextMenuStrip_1.Name = "ContextMenuStrip"
        Me.ContextMenuStrip_1.Size = New System.Drawing.Size(122, 26)
        '
        'DateTimePicker_Inizio
        '
        Me.DateTimePicker_Inizio.Checked = False
        Me.DateTimePicker_Inizio.CustomFormat = "dddd dd/MM/yyyy hh.mm.ss"
        Me.DateTimePicker_Inizio.Location = New System.Drawing.Point(104, 531)
        Me.DateTimePicker_Inizio.Name = "DateTimePicker_Inizio"
        Me.DateTimePicker_Inizio.ShowUpDown = True
        Me.DateTimePicker_Inizio.Size = New System.Drawing.Size(189, 20)
        Me.DateTimePicker_Inizio.TabIndex = 30
        '
        'BaseGriglia
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(792, 566)
        Me.Controls.Add(Me.DateTimePicker_Inizio)
        Me.Controls.Add(Me.TextBox_Nr_Record)
        Me.Controls.Add(Me.Label_Nr_Record)
        Me.Controls.Add(Me.ToolStrip)
        Me.Controls.Add(Me.DateTimePicker_Fine)
        Me.Controls.Add(Me.Label_DataFine)
        Me.Controls.Add(Me.Label_DataInizio)
        Me.Controls.Add(Me.DataGridView)
        Me.Name = "BaseGriglia"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "BaseGriglia"
        CType(Me.DataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStrip.ResumeLayout(False)
        Me.ToolStrip.PerformLayout()
        Me.ContextMenuStrip_1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DateTimePicker_Fine As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label_DataFine As System.Windows.Forms.Label
    Friend WithEvents Label_DataInizio As System.Windows.Forms.Label
    Friend WithEvents DataGridView As System.Windows.Forms.DataGridView
    Friend WithEvents ToolStrip As System.Windows.Forms.ToolStrip
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripButton_Nuovo As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripButton_Elimina As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripButton_Modifica As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripButton_Salva As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripButton_Annulla As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripButton_Stampa As System.Windows.Forms.ToolStripButton
    Friend WithEvents Label_Nr_Record As System.Windows.Forms.Label
    Friend WithEvents TextBox_Nr_Record As System.Windows.Forms.TextBox
    Friend WithEvents ToolStripSeparator8 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripButtonSelezionaTutto As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator9 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents StampaToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ContextMenuStrip_1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents DateTimePicker_Inizio As System.Windows.Forms.DateTimePicker
End Class
