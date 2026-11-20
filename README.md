# Semana 4 – Aplicaciones Web

Sistema de gestión de clubes deportivos hecho con C# (ASP.NET Core MVC) y SQL Server. Autor: Danny Javier López Contreras.

Las tablas `Clubes` y `Miembros` tienen los campos pedidos, y la tabla `Registros` inscribe a cada miembro en un club. Las clases `ClubDatos`, `MiembroDatos` y `RegistroDatos` hacen el CRUD con consultas parametrizadas.

## Cómo ejecutarlo

1. Crear la base de datos con `sqlcmd -S "(localdb)\MSSQLLocalDB" -i base_datos.sql -f 65001` o ejecutando `base_datos.sql` en SQL Server Management Studio.
2. En la carpeta del proyecto, ejecutar `dotnet run` (requiere el SDK de .NET 10).
3. Abrir http://localhost:5080.
