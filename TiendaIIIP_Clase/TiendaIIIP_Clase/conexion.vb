Imports System.Data
Imports System.Data.SqlClient
Imports System.Windows.Forms

Public Class conexion
    Public conexion As SqlConnection = New SqlConnection("Data Source= MEJIA09\TEW_SQLEXPRESS;Initial Catalog=TiendaIIIP; Integrated Security=True")
    Private cmb As SqlCommandBuilder
    Public ds As DataSet = New DataSet()
    Public da As SqlDataAdapter
    Public cmd As SqlCommand
    Public dr As SqlDataReader
    Public dv As New DataView

    Public Sub conectar()
        Try
            conexion.Open()
            MessageBox.Show("Conectado")
        Catch ex As Exception
            MessageBox.Show("Error al conectar")
        Finally
            conexion.Close()
        End Try
    End Sub

    Public Function LlenarTabla(ByVal sql, ByVal tabla)
        ds.Tables.Clear()
        da = New SqlDataAdapter(sql, conexion)
        cmb = New SqlCommandBuilder(da)
        da.Fill(ds, tabla)
        dv.Table = ds.Tables(0)
    End Function

    Public Function insertarUsuario(idUsuario As Integer, nombre As String, apellido As String, userName As String,
                                    passw As String, rol As String, estado As String, correo As String)
        Try
            conexion.Open()
            cmd = New SqlCommand("insertarUsuario", conexion)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@idUsuario", idUsuario)
            cmd.Parameters.AddWithValue("@nombre", nombre)
            cmd.Parameters.AddWithValue("@apellido", apellido)
            cmd.Parameters.AddWithValue("@userName", userName)
            cmd.Parameters.AddWithValue("@passw", passw)
            cmd.Parameters.AddWithValue("@rol", rol)
            cmd.Parameters.AddWithValue("@estado", estado)
            cmd.Parameters.AddWithValue("@correo", correo)
            If cmd.ExecuteNonQuery Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Public Function eliminarUsuario(idUsuario As Integer, rol As String)
        Try
            conexion.Open()
            cmd = New SqlCommand("eliminarUsuario", conexion)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@idUsuario", idUsuario)
            cmd.Parameters.AddWithValue("@rol", rol)
            If cmd.ExecuteNonQuery <> 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
            Return False
        Finally
            conexion.Close()
        End Try
    End Function

    Public Function modificarUsuario(idUsuario As Integer, nombre As String, apellido As String, userName As String,
                                    passw As String, rol As String, correo As String)
        Try
            conexion.Open()
            cmd = New SqlCommand("modificarUsuario", conexion)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@idUsuario", idUsuario)
            cmd.Parameters.AddWithValue("@nombre", nombre)
            cmd.Parameters.AddWithValue("@apellido", apellido)
            cmd.Parameters.AddWithValue("@userName", userName)
            cmd.Parameters.AddWithValue("@passw", passw)
            cmd.Parameters.AddWithValue("@rol", rol)
            cmd.Parameters.AddWithValue("@correo", correo)
            If cmd.ExecuteNonQuery Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Public Function buscarUsuario(ByVal condicion) As DataTable

        Try
            conexion.Open()
            Dim search As String = "select * from Usuario " + " where " + condicion

            Dim comand As New SqlCommand(search, conexion)
            If comand.ExecuteNonQuery Then
                Dim dataT As New DataTable
                Dim dataA As New SqlDataAdapter(comand)
                dataA.Fill(dataT)
                Return dataT
            Else
                Return Nothing
            End If
            conexion.Close()

        Catch ex As Exception
            MsgBox(ex.Message)
            Return Nothing
        End Try
    End Function
End Class
