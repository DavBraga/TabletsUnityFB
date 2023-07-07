using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalData 
{
    public static string adminID = "uoPQ6dmSQYQeDim9woXNp2ynNtJ2";
    public static string ConvertStatus(int value)
    {
        if(value==0)
        return "Funcionando";
        else if(value ==1)
        return "Com Defeito";
        else if(value ==2)
        return "Em Manutenção";
        else if(value== 3)
        return "Baixa";
        else 
        return "error";
    }
     public static string ConvertParadeiro(int value)
    {
        if(value==0)
        return "Com o responsável";
        else if(value ==1)
        return "Na Escola.";
        else if(value ==2)
        return "Na SME.";
        else if(value== 3)
        return "Desaparecido.";
        else 
        return "error";
    }
}
