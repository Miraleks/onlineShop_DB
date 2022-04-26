using System.Collections.Generic;

namespace eBay_DB.Models
{
    public class Order
        /*
        id - id записи
        Date - дата продажи
        Sum - сумма продажи (сумма стоимости продуктов в ордере)     
        */

    {

        void AddProduct(Product product, int value)
        {
            //добавление в OrderProduct
        }

        public long Id { get; set; }

        public System.DateTime Date { get; set; }

        public decimal Sum { get; set; }

    }
}
