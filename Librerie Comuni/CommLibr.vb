Imports System.Data.SqlClient

Module CommLibr

    Public Const LOG_OK = 10000
    Public Const LOG_WARNING = 20000
    Public Const LOG_ERROR = 30000

    Public Function AddLogEvent(ByVal iTipo As Integer, ByVal strDescrizione As String, ByVal ldLoginData As LoginData, ByVal strFormTitle As String, ByVal strConnStringParamName As String) As Boolean
        ' Nel caso non vi sia un utente loggato, prendo il primo della lista che deve essere quello di default
        ' L'utente di default
        Dim strConn, strSQL As String
        Dim iID_Utente As Integer
        iID_Utente = ldLoginData.iID_Utente
        If iID_Utente = 0 Then
            Try
                strConn = My.Settings.Item(strConnStringParamName)
                strSQL = "SELECT * FROM Utenti WHERE U_Utente='default' AND U_Password='default'AND U_Livello=0"
                Dim ds As New DataSet()
                Try
                    Dim da As New SqlDataAdapter(strSQL, strConn)
                    da.Fill(ds)
                    iID_Utente = ds.Tables(0).Rows(0).Item("U_ID")
                Catch ex As Exception

                    System.Windows.Forms.MessageBox.Show("Non e' stato configurato nessun utente di default oppure non e' stato ancora impostato un collegamento col DataBase.", strFormTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop)

                End Try

            Catch ex As Exception

                System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, strFormTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop)

            End Try
        End If

        Dim dtDateTime As Date = Date.Now
        Dim strNomeComputer As String
        Dim strNomeApplicazione As String
        strNomeComputer = My.Computer.Name
        strNomeApplicazione = My.Application.Info.ProductName
        strConn = My.Settings.Item(strConnStringParamName)
        strSQL = "INSERT INTO [LogEventi] "
        strSQL = strSQL + "  (LE_TL_ID,  LE_Nome,  LE_Descrizione,  LE_Data,  LE_ComputerName,  LE_U_ID) VALUES "
        strSQL = strSQL + "  (@LE_TL_ID, @LE_Nome, @LE_Descrizione, @LE_Data, @LE_ComputerName, @LE_U_ID)"

        Try

            Dim cn As New SqlConnection(strConn)
            cn.Open()
            Dim cmdInsert As New SqlCommand(strSQL, cn)
            cmdInsert.Parameters.AddWithValue("@LE_TL_ID", iTipo)
            cmdInsert.Parameters.AddWithValue("@LE_Nome", strNomeApplicazione)
            cmdInsert.Parameters.AddWithValue("@LE_Descrizione", strDescrizione)
            cmdInsert.Parameters.AddWithValue("@LE_Data", dtDateTime)
            cmdInsert.Parameters.AddWithValue("@LE_ComputerName", strNomeComputer)
            cmdInsert.Parameters.AddWithValue("@LE_U_ID", iID_Utente)

            cmdInsert.ExecuteNonQuery()
        Catch ex As Exception
            System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, strFormTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop)
        End Try

    End Function

    'Funzioni di accesso al database
    Public Function ConnettiDatabase(ByVal ldLoginData As LoginData, ByVal strFormTitle As String, ByVal strConnStringParamName As String) As Boolean
        ' Eseguo il test di connessione
        Dim DLG As New SQL_Connection_Dialog.SQL_Connection_Dialog
        DLG.Title = "Selezionare l'istanza SQL ed il database "
        DLG.ConnectionString = My.Settings.Item(strConnStringParamName)
        If DLG.ShowDialog <> Windows.Forms.DialogResult.Cancel Then

            DLG.SaveChange_To_App_Config(strConnStringParamName)
            'Update the settings
            My.MySettings.Default.Item(strConnStringParamName) = DLG.ConnectionString

        End If

        Return TestConnessione(ldLoginData, strFormTitle, strConnStringParamName)
    End Function

    Public Function TestConnessione(ByVal ldLoginData As LoginData, ByVal strFormTitle As String, ByVal strConnStringParamName As String) As Boolean
        Dim bRes As Boolean
        Dim strSQL As String
        Dim strConn As String

        bRes = False
        Try
            ' Verifico se SQL ha il database collegato
            strConn = My.Settings.Item(strConnStringParamName)
            strSQL = "SELECT * FROM Utenti"
            Dim da As New SqlDataAdapter(strSQL, strConn)
            Dim ds As New DataSet()
            da.Fill(ds)
            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    bRes = True
                End If
            End If
        Catch ex As Exception
            bRes = False
        End Try

        Return bRes

    End Function

    Public Sub ApriDataSet(ByVal strSQL As String, ByVal strTable As String, ByRef ds As DataSet, ByVal ldLoginData As LoginData, ByVal strFormTitle As String, ByVal strConnStringParamName As String)
        Dim strConn As String
        Dim bStrTableNameAlreadyPresent As Boolean
        strConn = My.Settings.Item(strConnStringParamName)
        bStrTableNameAlreadyPresent = False
        Try
            Using da As New SqlDataAdapter(strSQL, strConn)
                Try
                    If strTable = "" Then
                        If ds.Tables.Count > 0 Then
                            ds.Tables.Clear()
                        End If
                        da.Fill(ds)
                    Else
                        For Each dtTableName As DataTable In ds.Tables
                            If dtTableName.TableName = strTable Then
                                bStrTableNameAlreadyPresent = True
                            End If
                        Next dtTableName
                        If bStrTableNameAlreadyPresent = False Then
                            ds.Tables.Add(strTable)
                        End If
                        ds.Tables(strTable).Clear()
                        da.Fill(ds.Tables(strTable))
                    End If

                Catch ex As Exception
                    AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, ldLoginData, strFormTitle, strConnStringParamName)
                    System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, strFormTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                End Try

            End Using

        Catch ex As Exception
            AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, ldLoginData, strFormTitle, strConnStringParamName)
            System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, strFormTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop)
        End Try

    End Sub

    Public Function GENERICA_DESCRIZIONE_STRINGA(ByVal strColonnaDaCuiPrelevare As String, ByVal strTabella As String, ByVal strColonnaSelezione As String, ByVal strCodiceSelezione As String, ByVal ldLoginData As LoginData, ByVal strFormTitle As String, ByVal strConnStringParamName As String) As String
        Dim strSQL As String
        Dim ds As New DataSet()

        GENERICA_DESCRIZIONE_STRINGA = ""
        Try
            strSQL = "SELECT " + strColonnaDaCuiPrelevare + " FROM " + strTabella + " WHERE " + strColonnaSelezione + "= '" + strCodiceSelezione + "'"
            ApriDataSet(strSQL, "", ds, ldLoginData, strFormTitle, strConnStringParamName)
            For Each dr As DataRow In ds.Tables(0).Rows
                GENERICA_DESCRIZIONE_STRINGA = dr.Item(strColonnaDaCuiPrelevare).ToString
            Next dr

        Catch ex As Exception
            GENERICA_DESCRIZIONE_STRINGA = ""
            AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, ldLoginData, strFormTitle, strConnStringParamName)
            System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, strFormTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop)
        End Try

    End Function

    Public Function GENERICA_DESCRIZIONE_NUMERICA(ByVal strColonnaDaCuiPrelevare As String, ByVal strTabella As String, ByVal strColonnaSelezione As String, ByVal strCodiceSelezione As String, ByVal ldLoginData As LoginData, ByVal strFormTitle As String, ByVal strConnStringParamName As String) As Long
        Dim GENERICA_DESCRIZIONE_NUMERICA_TEMP As Long
        Dim strSQL As String
        Dim ds As New DataSet()

        GENERICA_DESCRIZIONE_NUMERICA = 0
        Try
            strSQL = "SELECT " + strColonnaDaCuiPrelevare + " FROM " + strTabella + " WHERE " + strColonnaSelezione + "= '" + strCodiceSelezione + "'"
            ApriDataSet(strSQL, "", ds, ldLoginData, strFormTitle, strConnStringParamName)
            For Each dr As DataRow In ds.Tables(0).Rows
                Try
                    GENERICA_DESCRIZIONE_NUMERICA_TEMP = System.Convert.ToInt32(dr.Item(strColonnaDaCuiPrelevare))
                Catch ex As Exception
                    GENERICA_DESCRIZIONE_NUMERICA_TEMP = 0
                End Try
                GENERICA_DESCRIZIONE_NUMERICA = GENERICA_DESCRIZIONE_NUMERICA_TEMP
                Exit For
            Next dr

        Catch ex As Exception
            GENERICA_DESCRIZIONE_NUMERICA = 0
            AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, ldLoginData, strFormTitle, strConnStringParamName)
            System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, "Fase Engineering srl", MessageBoxButtons.OK, MessageBoxIcon.Stop)
        End Try
    End Function

    ' Funzioni per il Copia/Incolla da codice
    <System.Runtime.InteropServices.DllImport("User32.dll", EntryPoint:="OpenClipboard", SetLastError:=True)> _
          Public Function OpenClipboard(ByVal hwnd As IntPtr) As Boolean
    End Function

    <System.Runtime.InteropServices.DllImport("User32.dll", EntryPoint:="IsClipboardFormatAvailable", SetLastError:=True)> _
       Public Function IsClipboardFormatAvailable(ByVal uint As UInteger) As Boolean
    End Function
    <System.Runtime.InteropServices.DllImport("User32.dll", EntryPoint:="GetClipboardData", SetLastError:=True)> _
       Public Function GetClipboardData(ByVal uint As UInteger) As IntPtr
    End Function

    <System.Runtime.InteropServices.DllImport("Kernel32.dll", EntryPoint:="GetLastError", SetLastError:=True)> _
           Public Function GetLastError() As UInteger
    End Function
End Module

Public Class LoginData

    Private m_iID_Utente As Integer
    Private m_strU_Utente As String
    Private m_strU_Password As String
    Private m_lLivello As Long
    Private m_strNome As String
    Private m_strCognome As String

    Private ld As LoginData

    Public Property Value() As LoginData
        Get
            ld.m_iID_Utente = m_iID_Utente
            ld.m_strU_Utente = m_strU_Utente
            ld.m_strU_Password = m_strU_Password
            ld.m_lLivello = m_lLivello
            ld.m_strNome = m_strNome
            ld.m_strCognome = m_strCognome

            Return ld
        End Get

        Set(ByVal ld As LoginData)
            m_iID_Utente = ld.m_iID_Utente
            m_strU_Utente = ld.m_strU_Utente
            m_strU_Password = ld.m_strU_Password
            m_lLivello = ld.m_lLivello
            m_strNome = ld.m_strNome
            m_strCognome = ld.m_strCognome
        End Set

    End Property

    Public Property iID_Utente() As Integer
        Get
            iID_Utente = m_iID_Utente
            Return iID_Utente
        End Get

        Set(ByVal iID_Utente As Integer)
            m_iID_Utente = iID_Utente
        End Set
    End Property

    Public Property strU_Utente() As String
        Get
            strU_Utente = m_strU_Utente
            Return strU_Utente
        End Get

        Set(ByVal strU_Utente As String)
            m_strU_Utente = strU_Utente
        End Set
    End Property

    Public Property strU_Password() As String
        Get
            strU_Password = m_strU_Password
            Return strU_Password
        End Get

        Set(ByVal strU_Password As String)
            m_strU_Password = strU_Password
        End Set
    End Property

    Public Property lLivello() As Long
        Get
            lLivello = m_lLivello
            Return lLivello
        End Get

        Set(ByVal lLivello As Long)
            m_lLivello = lLivello
        End Set
    End Property

    Public Property strNome() As String
        Get
            strNome = m_strNome
            Return strNome
        End Get

        Set(ByVal strNome As String)
            m_strNome = strNome
        End Set
    End Property

    Public Property strCognome() As String
        Get
            strCognome = m_strCognome
            Return strCognome
        End Get

        Set(ByVal strCognome As String)
            m_strCognome = strCognome
        End Set
    End Property

    Public Sub New(ByVal ld As LoginData)
        m_iID_Utente = ld.m_iID_Utente
        m_strU_Utente = ld.m_strU_Utente
        m_strU_Password = ld.m_strU_Password
        m_lLivello = ld.m_lLivello
        m_strNome = ld.m_strNome
        m_strCognome = ld.m_strCognome
    End Sub

    Public Sub New()
        m_iID_Utente = 0
        m_strU_Utente = ""
        m_strU_Password = ""
        m_lLivello = 0
        m_strNome = ""
        m_strCognome = ""
    End Sub

    Public Sub ResetValue()
        m_iID_Utente = 0
        m_strU_Utente = ""
        m_strU_Password = ""
        m_lLivello = 0
        m_strNome = ""
        m_strCognome = ""
    End Sub

End Class
