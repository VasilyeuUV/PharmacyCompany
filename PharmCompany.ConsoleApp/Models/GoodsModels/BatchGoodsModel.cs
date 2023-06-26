using System;
using System.ComponentModel;

namespace PharmCompany.ConsoleApp.Models
{
    public class BatchGoodsModel : ANameableEntityBase
    {

        /// <summary>
        ///  Guid Аптеки
        /// </summary>
        [DisplayName("Номер Склада")]
        public Guid StorageId { get; set; }


        /// <summary>
        ///  Guid Аптеки
        /// </summary>
        [DisplayName("Номер Товара")]
        public Guid GoodsId { get; set; }


        /// <summary>
        /// Хранилище для партии товаров
        /// </summary>
        public StorageModel Storage { get; set; }


        /// <summary>
        /// Товары в партии
        /// </summary>
        public GoodsModel Goods { get; set; }


        [DisplayName("Количество товара в партии")]
        public int GoodsCount { get; set; }
    }
}
