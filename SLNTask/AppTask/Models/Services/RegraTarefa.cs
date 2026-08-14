using AppTask.Models.Interface;
using System.Data;

namespace AppTask.Models.Services
{

    public class RegraTarefa : IRegraTarefa
    {
        public bool validarDataFinal(DateTime? datainicial, DateTime? datafinal)
        {
            return datainicial<datafinal;
        }
    }
}
