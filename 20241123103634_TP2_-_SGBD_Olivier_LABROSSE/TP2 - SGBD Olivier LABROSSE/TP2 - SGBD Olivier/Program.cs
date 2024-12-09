/*!
 * \file  "Program.cs"
 *
 * \brief 
 *     Programme cr�� automatiquement lors de la cr�ation du projet afin de lancer l'application en Winform
 *        
 * \author Olivier LABROSSE (cr�ation automatis�e)
 * \date 4/11/2024
 * \last update 4/11/2024
 *
 */


namespace TP2___SGBD_Olivier
{
    internal static class Program
    {            
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Affichagetable());
        }
    }
}