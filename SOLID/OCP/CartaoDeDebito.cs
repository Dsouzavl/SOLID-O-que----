using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCP
{
    public class CartaoDeDebito : IMeioDePagamento
    {
        public void LevantarFundos(decimal valorTotal)
        {
            //Retira valor total da conta do cliente
        }
    }
}
