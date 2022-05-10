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

        void AddProductToOrder(Product product, int value)
        {
            //добавление в OrderProduct
        }

        public long Id { get; set; }

        private System.DateTime CreatedAt { get; set; }
        

        public decimal Sum { get; set; }

        public override string ToString()
        {
            return $"Product: ID={this.Id}, Date: {this.CreatedAt}, Sum: {this.Sum}";
        }

    }
}
