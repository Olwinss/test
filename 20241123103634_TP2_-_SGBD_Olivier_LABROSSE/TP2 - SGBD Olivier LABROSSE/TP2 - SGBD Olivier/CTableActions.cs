/*!
 * \file  "CTableAction.cs"
 *
 * \brief 
 *      Fichier contenant les différentes classes permettant de gérer les actions sur les tables
 *      Ce fichier contient : 
 *       - La classe abstraite CTableAction permettant le polymorphisme des classes d'actions
 *       - Les classes CTableAjout, CTableModification et CTableSuppression héritant de CTableAction 
 *        
 * \author Olivier LABROSSE
 * \date 22/11/2024
 * \last update 23/11/2024
 *
 */

namespace TP2___SGBD_Olivier
{
    public abstract class CTableAction
    {
        static int idActionLibre = 1;
        protected CTable ligne;
        public int idAction;
        public TypeAction type { get; set; }
        public enum TypeAction
        {
            Ajout,
            Modification,
            Suppression
        }

        protected CTableAction()
        {
            idAction = idActionLibre++;
        }

        public CTable GetLigne()
        {
            return ligne;
        }

        public TypeAction GetTypeAction()
        {
            return type;
        }
    }

    public class CTableAjout : CTableAction
    {
        public CTableAjout(CTable table)
        {
            type = TypeAction.Ajout;
            ligne = table;
        }
    }
    public class CTableModification : CTableAction
    {
        CTable AncienneLigne;
        public CTableModification(CTable ancien, CTable nouveau)
        {
            AncienneLigne = ancien;
            type = TypeAction.Modification;
            ligne = nouveau;
        }

        public CTable GetAncienneLigne()
        {
            return AncienneLigne;
        }
    }
    public class CTableSuppression : CTableAction
    {
        public CTableSuppression(CTable table)
        {
            type = TypeAction.Suppression;
            ligne = table;
        }
    }
}
