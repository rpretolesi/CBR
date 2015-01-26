Imports System.Data.SqlClient

Public Class Main

    Public Const APP_TITLE = "Cooperativa Braccianti Riminese - Impianto di Pietracuta"

    Public m_ldLoginData As New LoginData

    Private Sub Main_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Per prima cosa creo o imposto la connessione
        If TestConnessione(m_ldLoginData, APP_TITLE, "db_ConnectionString") = False Then
            If ConnettiDatabase(m_ldLoginData, APP_TITLE, "db_ConnectionString") = False Then
                System.Windows.Forms.MessageBox.Show("Connessione al database non riuscita. L'applicazione verra' chiusa.", APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                Me.Close()
            End If
        End If
        Me.Text = APP_TITLE
        Me.Icon = Drawing.Icon.ExtractAssociatedIcon(My.Application.Info.DirectoryPath.ToString() + "\Icon.ico")

    End Sub

    Private Sub ImportaFileProduzioneToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ImportaFileProduzioneToolStripMenuItem.Click
        ' Seleziono il file
        Dim folderExists As Boolean
        If OpenFileDialog_1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            'Copio il file in 'C:\Prod500.dbf'
            Try
                AddLogEvent(LOG_OK, "Esecuzione copia file: " + OpenFileDialog_1.FileName + " in C:\cbr\Prod500.dbf.", m_ldLoginData, APP_TITLE, "db_ConnectionString")
                folderExists = My.Computer.FileSystem.DirectoryExists("C:\cbr")
                If folderExists = False Then
                    My.Computer.FileSystem.CreateDirectory("C:\cbr")
                End If

                My.Computer.FileSystem.CopyFile(OpenFileDialog_1.FileName, "C:\cbr\Prod500.dbf", True)
                AddLogEvent(LOG_OK, "Copia file OK.", m_ldLoginData, APP_TITLE, "db_ConnectionString")
            Catch ex As Exception
                AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, m_ldLoginData, APP_TITLE, "db_ConnectionString")
                AddLogEvent(LOG_ERROR, "Copia file Error", m_ldLoginData, APP_TITLE, "db_ConnectionString")
                System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End Try

            ' A questo punto, apro il form per gestire l'importazione
            GestImpProd.ShowDialog()

        End If

    End Sub

    Private Sub ImportaFileRicettePredosaggioToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ImportaFileRicettePredosaggioToolStripMenuItem.Click
        ' Seleziono il file
        Dim folderExists As Boolean
        If OpenFileDialog_1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            'Copio il file in 'C:\RicPrdsg.dbf'
            Try
                AddLogEvent(LOG_OK, "Esecuzione copia file: " + OpenFileDialog_1.FileName + " in C:\cbr\RicPrdsg.dbf.", m_ldLoginData, APP_TITLE, "db_ConnectionString")
                folderExists = My.Computer.FileSystem.DirectoryExists("C:\cbr")
                If folderExists = False Then
                    My.Computer.FileSystem.CreateDirectory("C:\cbr")
                End If

                My.Computer.FileSystem.CopyFile(OpenFileDialog_1.FileName, "C:\cbr\RicPrdsg.dbf", True)
                AddLogEvent(LOG_OK, "Copia file OK.", m_ldLoginData, APP_TITLE, "db_ConnectionString")
            Catch ex As Exception
                AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, m_ldLoginData, APP_TITLE, "db_ConnectionString")
                AddLogEvent(LOG_ERROR, "Copia file Error.", m_ldLoginData, APP_TITLE, "db_ConnectionString")
                System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End Try

            ' A questo punto, apro il form per gestire l'importazione
            GestImpRicPred.ShowDialog()

        End If
    End Sub

    Private Sub DatabaseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DatabaseToolStripMenuItem.Click
        If Login.ShowDialog(1, m_ldLoginData, Me.Icon, APP_TITLE + " - Login", "db_ConnectionString") = Windows.Forms.DialogResult.Yes Then
            If ConnettiDatabase(m_ldLoginData, APP_TITLE, "db_ConnectionString") = False Then
                System.Windows.Forms.MessageBox.Show("Connessione al database non riuscita. L'applicazione verra' chiusa.", APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                Me.Close()
            End If
        End If
    End Sub

    Private Sub JobToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles JobToolStripMenuItem.Click
        GestJob.Close()
        GestJob.MdiParent = Me
        Dim strSQLFROM As String
        strSQLFROM = "RepDosInt INNER JOIN RepDosPassi ON RepDosInt.RDI_ID = RepDosPassi.RDP_RDI_ID INNER JOIN RicDosInt ON RepDosInt.RDI_RIDI_ID = RicDosInt.RIDI_ID GROUP BY RIDI_ID,RIDI_Nome_Ricetta, RIDI_Nome_Ricetta_Predosaggio,RIDI_Job_Descrizione, RIDI_Data_Ora_Start,RIDI_Silo_Destinazione, RIDI_Rapporto_F_L"
        GestJob.Show("RIDI_ID,RIDI_Nome_Ricetta, RIDI_Nome_Ricetta_Predosaggio,RIDI_Job_Descrizione, RIDI_Data_Ora_Start,RIDI_Silo_Destinazione, RIDI_Rapporto_F_L, COUNT(RDI_ID)/20 as TotaleBatch, ROUND((SUM(RDP_Net_Kg_Prod)/1000), 1) as TotalePeso", strSQLFROM, "", "RIDI_Data_Ora_Start", "RIDI_ID", m_ldLoginData, Me.Icon, APP_TITLE + " - Gestione Job", "db_ConnectionString")
    End Sub

    Private Sub RicettePredosaggioToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RicettePredosaggioToolStripMenuItem.Click
        GestRicPreDos.Close()
        GestRicPreDos.MdiParent = Me
        GestRicPreDos.Show("*", "RicPreDosInt", "", "", "RPDI_ID", m_ldLoginData, Me.Icon, APP_TITLE + " - Ricette Predosaggio", "db_ConnectionString")
    End Sub

    Private Sub DosaggioToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DosaggioToolStripMenuItem.Click
        GestNomiDos.Close()
        GestNomiDos.MdiParent = Me
        GestNomiDos.Show("*", "NomiProdottiDosaggio", "", "", "NPD_ID", m_ldLoginData, Me.Icon, APP_TITLE + " - Nomi Dosaggio", "db_ConnectionString")
    End Sub

    Private Sub ProdottiPredosaggioToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProdottiPredosaggioToolStripMenuItem.Click
        GestNomiPreDos.Close()
        GestNomiPreDos.MdiParent = Me
        GestNomiPreDos.Show("*", "NomiProdottiPreDosaggio", "", "", "NPPD_ID", m_ldLoginData, Me.Icon, APP_TITLE + " - Nomi Predosaggio", "db_ConnectionString")
    End Sub

    Private Sub LogEventiToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LogEventiToolStripMenuItem.Click
        GestLogEventi.Close()
        GestLogEventi.MdiParent = Me
        GestLogEventi.Show("TipoLog.TL_Descrizione, LogEventi.LE_Nome, LogEventi.LE_Descrizione, LogEventi.LE_Data, LogEventi.LE_ComputerName, Utenti.U_Nome", "LogEventi INNER JOIN TipoLog ON LogEventi.LE_TL_ID = TipoLog.TL_ID INNER JOIN Utenti ON LogEventi.LE_U_ID = Utenti.U_ID", "", "LE_Data", "LE_ID", m_ldLoginData, Me.Icon, APP_TITLE + " - Log Eventi", "db_ConnectionString")
    End Sub

    Private Sub EliminaDatiProduzioneToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EliminaDatiProduzioneToolStripMenuItem.Click
        If Login.ShowDialog(8, m_ldLoginData, Me.Icon, APP_TITLE + " - Login", "db_ConnectionString") = Windows.Forms.DialogResult.Yes Then

            AddLogEvent(LOG_OK, "Esecuzione Elimina Dati Di Produzione.", m_ldLoginData, APP_TITLE, "db_ConnectionString")

            Dim bRes As Boolean

            bRes = True
            Try
                Dim strConn, strSQL As String

                Dim cn As New SqlConnection
                Dim cmdDelete As New SqlCommand

                strConn = My.Settings.Item("db_ConnectionString")
                cn.ConnectionString = strConn


                strSQL = "DELETE [RepDosPassi]"

                cmdDelete.CommandText = strSQL
                cmdDelete.Connection = cn

                cn.Open()

                Try
                    cmdDelete.ExecuteNonQuery()
                Catch ex As Exception
                    bRes = False

                    AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, m_ldLoginData, APP_TITLE, "db_ConnectionString")
                    System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                End Try
                cn.Close()


                strSQL = "DELETE [RepDosInt]"

                cmdDelete.CommandText = strSQL
                cmdDelete.Connection = cn

                cn.Open()

                Try
                    cmdDelete.ExecuteNonQuery()
                Catch ex As Exception
                    bRes = False

                    AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, m_ldLoginData, APP_TITLE, "db_ConnectionString")
                    System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                End Try
                cn.Close()


                strSQL = "DELETE [RicDosPassi]"

                cmdDelete.CommandText = strSQL
                cmdDelete.Connection = cn

                cn.Open()

                Try
                    cmdDelete.ExecuteNonQuery()
                Catch ex As Exception
                    bRes = False

                    AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, m_ldLoginData, APP_TITLE, "db_ConnectionString")
                    System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                End Try
                cn.Close()


                strSQL = "DELETE [RicDosInt]"

                cmdDelete.CommandText = strSQL
                cmdDelete.Connection = cn

                cn.Open()

                Try
                    cmdDelete.ExecuteNonQuery()
                Catch ex As Exception
                    bRes = False

                    AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, m_ldLoginData, APP_TITLE, "db_ConnectionString")
                    System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                End Try
                cn.Close()

            Catch ex As Exception
                bRes = False

                AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, m_ldLoginData, APP_TITLE, "db_ConnectionString")
                System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End Try

            If bRes = True Then
                AddLogEvent(LOG_OK, "Eliminazione Dati Di Produzione OK.", m_ldLoginData, APP_TITLE, "db_ConnectionString")
                System.Windows.Forms.MessageBox.Show("Eliminazione Dati Di Produzione OK.", APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                AddLogEvent(LOG_ERROR, "Errore nell'Eliminazione Dati Di Produzione.", m_ldLoginData, APP_TITLE, "db_ConnectionString")
                System.Windows.Forms.MessageBox.Show("Errore nell'Eliminazione Dati Di Produzione.", APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

        End If
    End Sub

    Private Sub EliminaNomiProdottoDosaggioToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EliminaNomiProdottoDosaggioToolStripMenuItem.Click
        If Login.ShowDialog(8, m_ldLoginData, Me.Icon, APP_TITLE + " - Login", "db_ConnectionString") = Windows.Forms.DialogResult.Yes Then

            AddLogEvent(LOG_OK, "Esecuzione Elimina Nomi Prodotto Dosaggio.", m_ldLoginData, APP_TITLE, "db_ConnectionString")
            Dim bRes As Boolean

            bRes = True
            Try
                Dim strConn, strSQL As String

                Dim cn As New SqlConnection
                Dim cmdDelete As New SqlCommand

                strConn = My.Settings.Item("db_ConnectionString")
                cn.ConnectionString = strConn


                strSQL = "DELETE [NomiProdottiDosaggio]"

                cmdDelete.CommandText = strSQL
                cmdDelete.Connection = cn

                cn.Open()

                Try
                    cmdDelete.ExecuteNonQuery()
                Catch ex As Exception
                    bRes = False

                    AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, m_ldLoginData, APP_TITLE, "db_ConnectionString")
                    System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                End Try
                cn.Close()

            Catch ex As Exception
                bRes = False

                AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, m_ldLoginData, APP_TITLE, "db_ConnectionString")
                System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End Try

            If bRes = True Then
                AddLogEvent(LOG_OK, "Eliminazione Nomi Prodotto Dosaggio OK.", m_ldLoginData, APP_TITLE, "db_ConnectionString")
                System.Windows.Forms.MessageBox.Show("Eliminazione Nomi Prodotto Dosaggio OK.", APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                AddLogEvent(LOG_ERROR, "Errore nell'Eliminazione Nomi Prodotto Dosaggio.", m_ldLoginData, APP_TITLE, "db_ConnectionString")
                System.Windows.Forms.MessageBox.Show("Errore nell'Eliminazione Nomi Prodotto Dosaggio.", APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If
    End Sub

    Private Sub EliminaRicettePredosaggioToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EliminaRicettePredosaggioToolStripMenuItem.Click
        If Login.ShowDialog(8, m_ldLoginData, Me.Icon, APP_TITLE + " - Login", "db_ConnectionString") = Windows.Forms.DialogResult.Yes Then
            AddLogEvent(LOG_OK, "Esecuzione Elimina Ricette Predosaggio.", m_ldLoginData, APP_TITLE, "db_ConnectionString")
            Dim bRes As Boolean

            bRes = True
            Try
                Dim strConn, strSQL As String

                Dim cn As New SqlConnection
                Dim cmdDelete As New SqlCommand

                strConn = My.Settings.Item("db_ConnectionString")
                cn.ConnectionString = strConn


                strSQL = "DELETE [RicPreDosPassi]"

                cmdDelete.CommandText = strSQL
                cmdDelete.Connection = cn

                cn.Open()

                Try
                    cmdDelete.ExecuteNonQuery()
                Catch ex As Exception
                    bRes = False

                    AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, m_ldLoginData, APP_TITLE, "db_ConnectionString")
                    System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                End Try
                cn.Close()


                strSQL = "DELETE [RicPreDosInt]"

                cmdDelete.CommandText = strSQL
                cmdDelete.Connection = cn

                cn.Open()

                Try
                    cmdDelete.ExecuteNonQuery()
                Catch ex As Exception
                    bRes = False

                    AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, m_ldLoginData, APP_TITLE, "db_ConnectionString")
                    System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                End Try
                cn.Close()

            Catch ex As Exception
                bRes = False

                AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, m_ldLoginData, APP_TITLE, "db_ConnectionString")
                System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End Try

            If bRes = True Then
                AddLogEvent(LOG_OK, "Eliminazione Ricette Predosaggio OK.", m_ldLoginData, APP_TITLE, "db_ConnectionString")
                System.Windows.Forms.MessageBox.Show("Eliminazione Ricette Predosaggio OK.", APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                AddLogEvent(LOG_ERROR, "Errore nell'Eliminazione Ricette Predosaggio.", m_ldLoginData, APP_TITLE, "db_ConnectionString")
                System.Windows.Forms.MessageBox.Show("Errore nell'Eliminazione Ricette Predosaggio.", APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If
    End Sub

    Private Sub EliminaNomiProdottoPredosaggioToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EliminaNomiProdottoPredosaggioToolStripMenuItem.Click
        If Login.ShowDialog(8, m_ldLoginData, Me.Icon, APP_TITLE + " - Login", "db_ConnectionString") = Windows.Forms.DialogResult.Yes Then
            AddLogEvent(LOG_OK, "Esecuzione Elimina Nomi Prodotto Predosaggio.", m_ldLoginData, APP_TITLE, "db_ConnectionString")
            Dim bRes As Boolean

            bRes = True
            Try
                Dim strConn, strSQL As String

                Dim cn As New SqlConnection
                Dim cmdDelete As New SqlCommand

                strConn = My.Settings.Item("db_ConnectionString")
                cn.ConnectionString = strConn


                strSQL = "DELETE [NomiProdottiPreDosaggio]"

                cmdDelete.CommandText = strSQL
                cmdDelete.Connection = cn

                cn.Open()

                Try
                    cmdDelete.ExecuteNonQuery()
                Catch ex As Exception
                    bRes = False

                    AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, m_ldLoginData, APP_TITLE, "db_ConnectionString")
                    System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                End Try
                cn.Close()

            Catch ex As Exception
                bRes = False

                AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, m_ldLoginData, APP_TITLE, "db_ConnectionString")
                System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End Try

            If bRes = True Then
                AddLogEvent(LOG_OK, "Eliminazione Nomi Prodotto Predosaggio OK.", m_ldLoginData, APP_TITLE, "db_ConnectionString")
                System.Windows.Forms.MessageBox.Show("Eliminazione Nomi Prodotto Predosaggio OK.", APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                AddLogEvent(LOG_ERROR, "Errore nell'Eliminazione Nomi Prodotto Predosaggio.", m_ldLoginData, APP_TITLE, "db_ConnectionString")
                System.Windows.Forms.MessageBox.Show("Errore nell'Eliminazione Nomi Prodotto Predosaggio.", APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If
    End Sub

    Private Sub EliminaFileDiLogToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EliminaFileDiLogToolStripMenuItem.Click
        If Login.ShowDialog(1, m_ldLoginData, Me.Icon, APP_TITLE + " - Login", "db_ConnectionString") = Windows.Forms.DialogResult.Yes Then
            AddLogEvent(LOG_OK, "Esecuzione Elimina Log Eventi.", m_ldLoginData, APP_TITLE, "db_ConnectionString")
            Dim bRes As Boolean

            bRes = True
            Try
                Dim strConn, strSQL As String

                Dim cn As New SqlConnection
                Dim cmdDelete As New SqlCommand

                strConn = My.Settings.Item("db_ConnectionString")
                cn.ConnectionString = strConn


                strSQL = "DELETE [LogEventi]"

                cmdDelete.CommandText = strSQL
                cmdDelete.Connection = cn

                cn.Open()

                Try
                    cmdDelete.ExecuteNonQuery()
                Catch ex As Exception
                    bRes = False

                    AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, m_ldLoginData, APP_TITLE, "db_ConnectionString")
                    System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                End Try
                cn.Close()

            Catch ex As Exception
                bRes = False

                AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, m_ldLoginData, APP_TITLE, "db_ConnectionString")
                System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End Try

            If bRes = True Then
                AddLogEvent(LOG_OK, "Eliminazione Log Eventi OK.", m_ldLoginData, APP_TITLE, "db_ConnectionString")
                System.Windows.Forms.MessageBox.Show("Eliminazione Log Eventi OK.", APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                AddLogEvent(LOG_ERROR, "Errore nell'Eliminazione Log Eventi.", m_ldLoginData, APP_TITLE, "db_ConnectionString")
                System.Windows.Forms.MessageBox.Show("Errore nell'Eliminazione Log Eventi.", APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If
    End Sub

End Class
