using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalData 
{
    public static string neutralColor = "#3EA1B9";
    public static string goodColor = "#3EB941";

    public static string alertColor = "#C1BF41";
    public static string problemColor ="#C12F3B";
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
