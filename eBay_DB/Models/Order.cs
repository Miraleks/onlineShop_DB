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

        private System.DateTime date;
        public System.DateTime Date 
        {
            get { return date; }
            set { date = System.DateTime.Parse(value.ToString()); } 
        }

        public decimal Sum { get; set; }

        public override string ToString()
        {
            return $"Product: ID={this.Id}, Date: {this.Date}, Sum: {this.Sum}";
        }

    }
}
