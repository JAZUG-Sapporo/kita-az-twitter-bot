using System;
using System.Linq;

namespace myBot.Models.forScripting
{
    /// <summary>
    /// 拡張スクリプト中でアクセス可能なメンバを制限するための、Message 型に対するラッパーです。
    /// </summary>
    public class MessageWrapper
    {
        internal Message Object { get; private set; }

        internal int MessageID => Object.MessageID;

        public string Text
        {
            get { return this.Object.Text; }
            set { this.Object.Text = value; }
        }

        public int Order
        {
            get { return this.Object.Order; }
            set { this.Object.Order = value; }
        }

        public bool IsArchived => this.Object.IsArchived;

        public MessageWrapper(Message message)
        {
            this.Object = message;
        }
    }
}