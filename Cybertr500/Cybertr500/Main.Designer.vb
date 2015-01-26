<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Main
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Main))
        Me.MainMenu = New System.Windows.Forms.MenuStrip
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ImportaFileProduzioneToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.ImportaFileRicettePredosaggioToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.DatabaseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ProduzioneToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.JobToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.RicettePredosaggioToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.DefinizioneNomiToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.DosaggioToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.DefinizioneUmiditaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ProdottiPredosaggioToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.LogToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.LogEventiToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ManutenzioneToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.EliminaDatiProduzioneToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.EliminaNomiProdottoDosaggioToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.EliminaRicettePredosaggioToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.EliminaNomiProdottoPredosaggioToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.EliminaFileDiLogToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OpenFileDialog_1 = New System.Windows.Forms.OpenFileDialog
        Me.MainMenu.SuspendLayout()
        Me.SuspendLayout()
        '
        'MainMenu
        '
        Me.MainMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.ProduzioneToolStripMenuItem, Me.DefinizioneNomiToolStripMenuItem, Me.DefinizioneUmiditaToolStripMenuItem, Me.LogToolStripMenuItem, Me.ManutenzioneToolStripMenuItem})
        Me.MainMenu.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu.Name = "MainMenu"
        Me.MainMenu.Size = New System.Drawing.Size(881, 24)
        Me.MainMenu.TabIndex = 3
        Me.MainMenu.Text = "MainMenu"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ImportaFileProduzioneToolStripMenuItem, Me.ToolStripSeparator1, Me.ImportaFileRicettePredosaggioToolStripMenuItem, Me.ToolStripSeparator2, Me.DatabaseToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(35, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'ImportaFileProduzioneToolStripMenuItem
        '
        Me.ImportaFileProduzioneToolStripMenuItem.Name = "ImportaFileProduzioneToolStripMenuItem"
        Me.ImportaFileProduzioneToolStripMenuItem.Size = New System.Drawing.Size(241, 22)
        Me.ImportaFileProduzioneToolStripMenuItem.Text = "Importa File Produzione"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(238, 6)
        '
        'ImportaFileRicettePredosaggioToolStripMenuItem
        '
        Me.ImportaFileRicettePredosaggioToolStripMenuItem.Name = "ImportaFileRicettePredosaggioToolStripMenuItem"
        Me.ImportaFileRicettePredosaggioToolStripMenuItem.Size = New System.Drawing.Size(241, 22)
        Me.ImportaFileRicettePredosaggioToolStripMenuItem.Text = "Importa File Ricette Predosaggio"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(238, 6)
        '
        'DatabaseToolStripMenuItem
        '
        Me.DatabaseToolStripMenuItem.Name = "DatabaseToolStripMenuItem"
        Me.DatabaseToolStripMenuItem.Size = New System.Drawing.Size(241, 22)
        Me.DatabaseToolStripMenuItem.Text = "Seleziona Database SQL"
        '
        'ProduzioneToolStripMenuItem
        '
        Me.ProduzioneToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.JobToolStripMenuItem, Me.RicettePredosaggioToolStripMenuItem})
        Me.ProduzioneToolStripMenuItem.Name = "ProduzioneToolStripMenuItem"
        Me.ProduzioneToolStripMenuItem.Size = New System.Drawing.Size(105, 20)
        Me.ProduzioneToolStripMenuItem.Text = "Analisi Produzione"
        '
        'JobToolStripMenuItem
        '
        Me.JobToolStripMenuItem.Name = "JobToolStripMenuItem"
        Me.JobToolStripMenuItem.Size = New System.Drawing.Size(181, 22)
        Me.JobToolStripMenuItem.Text = "Job"
        '
        'RicettePredosaggioToolStripMenuItem
        '
        Me.RicettePredosaggioToolStripMenuItem.Name = "RicettePredosaggioToolStripMenuItem"
        Me.RicettePredosaggioToolStripMenuItem.Size = New System.Drawing.Size(181, 22)
        Me.RicettePredosaggioToolStripMenuItem.Text = "Ricette Predosaggio"
        '
        'DefinizioneNomiToolStripMenuItem
        '
        Me.DefinizioneNomiToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DosaggioToolStripMenuItem})
        Me.DefinizioneNomiToolStripMenuItem.Name = "DefinizioneNomiToolStripMenuItem"
        Me.DefinizioneNomiToolStripMenuItem.Size = New System.Drawing.Size(97, 20)
        Me.DefinizioneNomiToolStripMenuItem.Text = "Definizione Nomi"
        '
        'DosaggioToolStripMenuItem
        '
        Me.DosaggioToolStripMenuItem.Name = "DosaggioToolStripMenuItem"
        Me.DosaggioToolStripMenuItem.Size = New System.Drawing.Size(129, 22)
        Me.DosaggioToolStripMenuItem.Text = "Dosaggio"
        '
        'DefinizioneUmiditaToolStripMenuItem
        '
        Me.DefinizioneUmiditaToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ProdottiPredosaggioToolStripMenuItem})
        Me.DefinizioneUmiditaToolStripMenuItem.Name = "DefinizioneUmiditaToolStripMenuItem"
        Me.DefinizioneUmiditaToolStripMenuItem.Size = New System.Drawing.Size(111, 20)
        Me.DefinizioneUmiditaToolStripMenuItem.Text = "Definizione Umidita'"
        '
        'ProdottiPredosaggioToolStripMenuItem
        '
        Me.ProdottiPredosaggioToolStripMenuItem.Name = "ProdottiPredosaggioToolStripMenuItem"
        Me.ProdottiPredosaggioToolStripMenuItem.Size = New System.Drawing.Size(185, 22)
        Me.ProdottiPredosaggioToolStripMenuItem.Text = "Prodotti Predosaggio"
        '
        'LogToolStripMenuItem
        '
        Me.LogToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LogEventiToolStripMenuItem})
        Me.LogToolStripMenuItem.Name = "LogToolStripMenuItem"
        Me.LogToolStripMenuItem.Size = New System.Drawing.Size(36, 20)
        Me.LogToolStripMenuItem.Text = "Log"
        '
        'LogEventiToolStripMenuItem
        '
        Me.LogEventiToolStripMenuItem.Name = "LogEventiToolStripMenuItem"
        Me.LogEventiToolStripMenuItem.Size = New System.Drawing.Size(135, 22)
        Me.LogEventiToolStripMenuItem.Text = "Log Eventi"
        '
        'ManutenzioneToolStripMenuItem
        '
        Me.ManutenzioneToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EliminaDatiProduzioneToolStripMenuItem, Me.EliminaNomiProdottoDosaggioToolStripMenuItem, Me.EliminaRicettePredosaggioToolStripMenuItem, Me.EliminaNomiProdottoPredosaggioToolStripMenuItem, Me.EliminaFileDiLogToolStripMenuItem})
        Me.ManutenzioneToolStripMenuItem.Name = "ManutenzioneToolStripMenuItem"
        Me.ManutenzioneToolStripMenuItem.Size = New System.Drawing.Size(86, 20)
        Me.ManutenzioneToolStripMenuItem.Text = "Manutenzione"
        '
        'EliminaDatiProduzioneToolStripMenuItem
        '
        Me.EliminaDatiProduzioneToolStripMenuItem.Name = "EliminaDatiProduzioneToolStripMenuItem"
        Me.EliminaDatiProduzioneToolStripMenuItem.Size = New System.Drawing.Size(250, 22)
        Me.EliminaDatiProduzioneToolStripMenuItem.Text = "Elimina Dati Produzione"
        '
        'EliminaNomiProdottoDosaggioToolStripMenuItem
        '
        Me.EliminaNomiProdottoDosaggioToolStripMenuItem.Name = "EliminaNomiProdottoDosaggioToolStripMenuItem"
        Me.EliminaNomiProdottoDosaggioToolStripMenuItem.Size = New System.Drawing.Size(250, 22)
        Me.EliminaNomiProdottoDosaggioToolStripMenuItem.Text = "Elimina Nomi Prodotto Dosaggio"
        '
        'EliminaRicettePredosaggioToolStripMenuItem
        '
        Me.EliminaRicettePredosaggioToolStripMenuItem.Name = "EliminaRicettePredosaggioToolStripMenuItem"
        Me.EliminaRicettePredosaggioToolStripMenuItem.Size = New System.Drawing.Size(250, 22)
        Me.EliminaRicettePredosaggioToolStripMenuItem.Text = "Elimina Ricette Predosaggio"
        '
        'EliminaNomiProdottoPredosaggioToolStripMenuItem
        '
        Me.EliminaNomiProdottoPredosaggioToolStripMenuItem.Name = "EliminaNomiProdottoPredosaggioToolStripMenuItem"
        Me.EliminaNomiProdottoPredosaggioToolStripMenuItem.Size = New System.Drawing.Size(250, 22)
        Me.EliminaNomiProdottoPredosaggioToolStripMenuItem.Text = "Elimina Nomi Prodotto Predosaggio"
        '
        'EliminaFileDiLogToolStripMenuItem
        '
        Me.EliminaFileDiLogToolStripMenuItem.Name = "EliminaFileDiLogToolStripMenuItem"
        Me.EliminaFileDiLogToolStripMenuItem.Size = New System.Drawing.Size(250, 22)
        Me.EliminaFileDiLogToolStripMenuItem.Text = "Elimina File Di Log"
        '
        'OpenFileDialog_1
        '
        Me.OpenFileDialog_1.Filter = "File dBase  (*.dbf)|*.dbf"
        Me.OpenFileDialog_1.Title = """Seleziona il file da Importare ..."""
        '
        'Main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = CType(resources.GetObject("$this.BackgroundImage"), System.Drawing.Image)
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.ClientSize = New System.Drawing.Size(881, 384)
        Me.Controls.Add(Me.MainMenu)
        Me.IsMdiContainer = True
        Me.Name = "Main"
        Me.MainMenu.ResumeLayout(False)
        Me.MainMenu.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MainMenu As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ImportaFileProduzioneToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents DatabaseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LogToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LogEventiToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenFileDialog_1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ImportaFileRicettePredosaggioToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ProduzioneToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents JobToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DefinizioneNomiToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DosaggioToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RicettePredosaggioToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ManutenzioneToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EliminaDatiProduzioneToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EliminaNomiProdottoDosaggioToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EliminaRicettePredosaggioToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EliminaNomiProdottoPredosaggioToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EliminaFileDiLogToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DefinizioneUmiditaToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ProdottiPredosaggioToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

End Class
