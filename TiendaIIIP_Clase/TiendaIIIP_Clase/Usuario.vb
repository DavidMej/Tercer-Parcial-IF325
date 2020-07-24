Imports System.Text.RegularExpressions
Imports System.Security.Cryptography
Imports System.Text

Public Class Usuario
    Dim conexion As New conexion()
    Dim dt As New DataTable
    Private Sub Usuario_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        conexion.conectar()
        llenarHistorial()
        txtPswEncrip.Visible = False
    End Sub
    Private Function validarCorreo(ByVal isCorreo As String) As Boolean
        Return Regex.IsMatch(isCorreo, "^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$")
    End Function


    Private Sub limpiar()
        txtCodigo.Clear()
        txtNombre.Clear()
        txtApellido.Clear()
        txtUserName.Clear()
        txtPsw.Clear()
        txtCorreo.Clear()
        cmbRol.ResetText()
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If validarCorreo(LCase(txtCorreo.Text)) = False Then
            MessageBox.Show("Correo invalido, *username@midominio.com*", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            txtCorreo.Focus()
            txtCorreo.SelectAll()
        Else
            insertarUsuaurio()
            'MessageBox.Show("Correo valido", "Correcto", MessageBoxButtons.OK, MessageBoxIcon.Information)
            conexion.conexion.Close()
        End If


    End Sub
    Private Sub insertarUsuaurio()
        Dim idUsuario As Integer
        Dim nombre, apellido, userName, passw, correo, rol, estado As String
        idUsuario = txtCodigo.Text
        nombre = txtNombre.Text
        apellido = txtApellido.Text
        userName = txtUserName.Text
        passw = txtPsw.Text
        correo = txtCorreo.Text
        estado = "activo"
        rol = cmbRol.Text
        Try
            If conexion.insertarUsuario(idUsuario, nombre, apellido, userName, passw, rol, estado, correo) Then
                MessageBox.Show("Guardado", "Correcto", MessageBoxButtons.OK, MessageBoxIcon.Information)
                limpiar()
            Else
                MessageBox.Show("Error al guardar", "Incorrecto", MessageBoxButtons.OK, MessageBoxIcon.Error)
                conexion.conexion.Close()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub llenarHistorial()
        Dim estado As String
        Dim pswencrip As String
        pswencrip = txtPswEncrip.Text
        estado = "activo"
        conexion.LlenarTabla("Select idUsuario as 'Id Usuario', nombre as 'Nombre', apellido as 'Apellido', nombreUsuario as 'User Name', passw as 'Password', correo as 'Correo', rol as 'Rol', estado as 'Estado' from Usuario", "Usuario")
        dgvRegistros.DataSource = conexion.ds.Tables("Usuario")
    End Sub

    Private Sub dgvRegistros_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvRegistros.CellClick
        txtCodigo.Text = dgvRegistros.CurrentRow.Cells(0).Value.ToString
        txtNombre.Text = dgvRegistros.CurrentRow.Cells(1).Value.ToString
        txtApellido.Text = dgvRegistros.CurrentRow.Cells(2).Value.ToString
        txtUserName.Text = dgvRegistros.CurrentRow.Cells(3).Value.ToString
        txtPsw.Text = dgvRegistros.CurrentRow.Cells(4).Value.ToString
        txtCorreo.Text = dgvRegistros.CurrentRow.Cells(5).Value.ToString
        cmbRol.Text = dgvRegistros.CurrentRow.Cells(6).Value.ToString
    End Sub

    Private Sub eliminarUsuario()
        Dim idUsuario As String
        Dim rol As String
        idUsuario = txtCodigo.Text
        rol = cmbRol.Text
        Try
            If (conexion.eliminarUsuario(idUsuario, rol)) Then
                MsgBox("Dado de baja")
                'conexion.conexion.Close()
            Else
                MsgBox("Error al dar de baja usuario")
                'conexion.conexion.Close()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub buscarUsuario()
        Try
            dt = conexion.buscarUsuario("nombreUsuario like '%" + txtUserName.Text + "%'")
            If dt.Rows.Count <> 0 Then
                dgvRegistros.DataSource = dt
                conexion.conexion.Close()
            Else
                dgvRegistros.DataSource = Nothing
                conexion.conexion.Close()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub btnLimpiar_Click(sender As Object, e As EventArgs) Handles btnLimpiar.Click
        limpiar()
    End Sub


    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        eliminarUsuario()
    End Sub

    Private Sub btnModificar_Click(sender As Object, e As EventArgs) Handles btnModificar.Click
        Dim idUsuario As Integer
        Dim nombre, apellido, userName, passw, correo, rol, estado As String
        idUsuario = txtCodigo.Text
        nombre = txtNombre.Text
        cambia(nombre)
        apellido = txtApellido.Text
        cambia(apellido)
        userName = txtUserName.Text
        passw = txtPsw.Text
        correo = txtCorreo.Text
        estado = "activo"
        rol = cmbRol.Text
        Try
            If conexion.modificarUsuario(idUsuario, nombre, apellido, userName, passw, rol, correo) Then
                MessageBox.Show("El Usuario se modifico correctamente", "Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information)
                limpiar()
            Else
                MessageBox.Show("Error al modificar el Usuario", "Usuario", MessageBoxButtons.OK, MessageBoxIcon.Error)
                conexion.conexion.Close()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        buscarUsuario()
        limpiar()
    End Sub

    Private Sub btnVer_Click(sender As Object, e As EventArgs) Handles btnVer.Click
        llenarHistorial()
    End Sub



    'Public Function Encriptar(ByVal Input As String) As String

    '    Dim IV() As Byte = ASCIIEncoding.ASCII.GetBytes("qualityi")
    '    Dim EncryptionKey() As Byte = Convert.FromBase64String("rpaSPvIvVLlrcmtzPU9/c67Gkj7yL1S5")
    '    Dim buffer() As Byte = Encoding.UTF8.GetBytes(Input)
    '    Dim des As TripleDESCryptoServiceProvider = New TripleDESCryptoServiceProvider
    '    des.Key = EncryptionKey
    '    des.IV = IV

    '    Return Convert.ToBase64String(des.CreateEncryptor().TransformFinalBlock(buffer, 0, buffer.Length()))

    'End Function

    'Public Function Desencriptar(ByVal Input As String) As String

    '    Dim IV() As Byte = ASCIIEncoding.ASCII.GetBytes("qualityi")
    '    Dim EncryptionKey() As Byte = Convert.FromBase64String("rpaSPvIvVLlrcmtzPU9/c67Gkj7yL1S5")
    '    Dim buffer() As Byte = Convert.FromBase64String(Input)
    '    Dim des As TripleDESCryptoServiceProvider = New TripleDESCryptoServiceProvider
    '    des.Key = EncryptionKey
    '    des.IV = IV
    '    Return Encoding.UTF8.GetString(des.CreateDecryptor().TransformFinalBlock(buffer, 0, buffer.Length()))

    'End Function

    'Private Sub btnEncriptar_Click(sender As Object, e As EventArgs) Handles btnEncriptar.Click
    '    Encriptar(txtPsw.Text)
    '    btnDesencriptar.Visible = True
    '    btnEncriptar.Visible = False
    'End Sub

    'Private Sub btnDesencriptar_Click(sender As Object, e As EventArgs) Handles btnDesencriptar.Click
    '    Desencriptar(txtPsw.Text)
    '    btnDesencriptar.Visible = False
    '    btnEncriptar.Visible = True
    'End Sub

    Private Function cambia(ByVal cambiatext As String) As String
        Dim a As String = StrConv(cambiatext, VbStrConv.ProperCase)
        Return a
    End Function

    Private Sub btnEncriptar_Click(sender As Object, e As EventArgs) Handles btnEncriptar.Click
        txtPswEncrip.Visible = True
        Dim cadena As String
        cadena = UCase(txtPsw.Text)
        For i = 1 To Len(cadena)
            txtPswEncrip.Text = txtPswEncrip.Text & "0" & Asc(Mid(cadena, i, 1))
        Next i
    End Sub

    Private Sub btnDesencriptar_Click(sender As Object, e As EventArgs) Handles btnDesencriptar.Click
        Dim cadena As String
        cadena = txtPswEncrip.Text
        txtPswEncrip.Text = ""
        For i = 1 To Len(cadena) Step 3
            txtPswEncrip.Text = txtPswEncrip.Text & Chr(Val(Mid(cadena, i, 3)))
        Next i
    End Sub
End Class
