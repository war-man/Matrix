﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataService.Context;
using DataService.Entities;
using DataService.ViewModel;

namespace DataService
{
    /// <summary>
    /// Modelisateur des donnees 
    /// </summary>
    public class ModelService
    {
             
        /// <summary>
        /// Renvoi la list des departements avec leur employer
        /// </summary>
        /// <returns></returns>
        public List<DepStaffCard> GetDepStaffsCard ( )
        {                       
            using (var Db = new EF())
            {
                var DepStaffCardList = new List<DepStaffCard> {  };
                var Nd = new DepStaffCard("");

                if (Nd.STAFFS_LIST.Any()) { DepStaffCardList.Add(Nd);}

                var Deps = (from S in Db.STAFF.ToList() where S.DEPARTEMENT != null select S.DEPARTEMENT).Distinct().ToList();

                Parallel.ForEach(Deps, Dep =>
                {
                    DepStaffCardList.Add(new DepStaffCard(Dep));
                });

                return DepStaffCardList.Any() ? DepStaffCardList.OrderBy(D => D.DEPARTEMENT_NAME).ToList() : DepStaffCardList;
            }       
        }

        /// <summary>
        /// Renvoi les filieres avec leurs classes
        /// </summary>
        /// <returns></returns>
        public List<FiliereClassCard> GetFiliereClassCards ( )
        {
            using (var Db = new EF())
            {
                var Fls = Db.FILIERE;

                var ClassCardList = new List<FiliereClassCard>();
                
                Parallel.ForEach(Fls, Fil =>
                {
                    var FC = new FiliereClassCard(Fil);
                    if (FC.CLASS_LIST.Any()) { ClassCardList.Add(FC); }
                });

                return ClassCardList.Any() ? ClassCardList.OrderBy(F => F.FILIERE_NAME).ToList() : ClassCardList;
            }           
        }
        
        /// <summary>
        /// renvoi la filiere avec ses classes
        /// </summary>
        /// <param name="FiliereID"></param>
        /// <returns></returns>
        public List<ClassCard> GetFiliereClassCards ( Guid FiliereID )
        {                        
            using (var Db = new EF())
            {
                var Class_List = new List<ClassCard> ();

                Parallel.ForEach(Db.CLASSE.Where(C => C.FILIERE_ID == FiliereID), C =>
                {
                    Class_List.Add (new ClassCard (C));
                });

                return Class_List.Any()? Class_List.OrderBy(C => C.LEVEL).ToList() : Class_List;
            }
        }

        /// <summary>
        /// Renvoi les informations des cours d'une classe pour une semaine
        /// </summary>
        /// <param name="classID">ID de la Classe</param>
        /// <param name="scheduleDate">Une date de cette Semaine</param>
        /// <returns></returns>
        public List<DayCoursCards> GetClassWeekAgendaData ( Guid classID, DateTime scheduleDate )
        {
            scheduleDate = scheduleDate.Date; 

            var FirstDateOfWeek = scheduleDate.DayOfWeek == DayOfWeek.Sunday ? scheduleDate.AddDays(-6) : scheduleDate.AddDays (-((int)scheduleDate.DayOfWeek - 1));
          
            var ScheduleData = new List<DayCoursCards>();

            for (var i = 0; i <= 6; i++)
            {
                var DayCard = new DayCoursCards(classID, FirstDateOfWeek.AddDays(i));

                if (DayCard.DAY_COURS.Any())
                {
                    ScheduleData.Add(DayCard);
                }                
            }
 
            return ScheduleData;
        }

        /// <summary>
        /// Renvoi les Matieres Cards Pour Une Classe
        /// </summary>
        /// <param name="ClasseID">ID de la Classe</param>
        /// <returns></returns>
        public List<MatiereCard> GetClassMatieresCards(Guid ClasseID)
        {
            using (var Db = new EF())
            {
                var MatierCard_List = new List<MatiereCard>();

                Parallel.ForEach(Db.MATIERE.Where(M => M.CLASSE_ID == ClasseID), MC =>
                {
                    MatierCard_List.Add(new MatiereCard(MC));
                });

                return MatierCard_List.Any() ? MatierCard_List.OrderBy(M => M?.NAME).ToList() : MatierCard_List;
            }
        }







    }
 
}









//private readonly DbService DS = new DbService ();

//public List<FiliereCard> GetAllFilieresCards ( )
//{
//    using(var Db = new EF ())
//    {
//        var FL = new List<FiliereCard> ();

//        Parallel.ForEach (Db.FILIERE, F =>
//        {
//            FL.Add (new FiliereCard (F));
//        });
//        return FL;
//    }
//}

//public List<FiliereLevelCard> GetFiliereMatieresCards ( Guid FiliereID )
//{

//    var MatiereCardList = new List<FiliereLevelCard> ();
//    var Ds = new DbService();

//    foreach(int Level in Ds.GetFILIERE_NIVEAUX (FiliereID))
//    {
//        MatiereCardList.Add (new FiliereLevelCard (FiliereID, Level));
//    }
//    return MatiereCardList;                       
//}


//public List<MatiereCard> GetClassMatieresCards ( Classe MyClasse )
//{                        
//    using(var Db = new EF ())
//    {
//        var MATIERES_LIST = new List<MatiereCard> ();
//        foreach(var M in Db.MATIERE.Where (M => M.FILIERE_ID == MyClasse.FILIERE_ID && M.FILIERE_LEVEL == MyClasse.LEVEL))
//        {
//            MATIERES_LIST.Add (new MatiereCard (M));
//        }
//        return MATIERES_LIST;
//    }
//}
