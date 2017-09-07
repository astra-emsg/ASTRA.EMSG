namespace ASTRA.EMSG.Common.Enums
{
    // do not rename values, used in asp.net identity role table
    public enum Rolle
    {
        // mandant roles
        DataManager,                //DataMgr
        DataReader,                 //User
        Benutzeradministrator,      //AdminMand
        Benchmarkteilnehmer,        //Benchmark

        // global roles
        Applikationsadministrator,  //ConfMgr
        Applikationssupporter       //ProbMgr
    }
}