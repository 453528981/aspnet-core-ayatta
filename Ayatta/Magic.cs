namespace Ayatta
{
    /// <summary>
    /// 魔法类
    /// </summary>
    /// <typeparam name="TA"></typeparam>
    public class Magic<TA>
    {

        public TA First { get; set; }

        public Magic()
            : this(default(TA))
        {

        }

        public Magic(TA first)
        {
            First = first;
        }
    }

    /// <summary>
    /// 魔法类
    /// </summary>
    /// <typeparam name="TA"></typeparam>
    /// <typeparam name="TB"></typeparam>
    public class Magic<TA, TB> : Magic<TA>
    {
        public TB Second { get; set; }

        public Magic()
            : this(default(TA))
        {

        }

        public Magic(TA first)
            : this(first, default(TB))
        {

        }

        public Magic(TA first, TB second)
        {
            First = first;
            Second = second;
        }
    }

    /// <summary>
    /// 魔法类
    /// </summary>
    /// <typeparam name="TA"></typeparam>
    /// <typeparam name="TB"></typeparam>
    /// <typeparam name="TC"></typeparam>
    public class Magic<TA, TB, TC> : Magic<TA, TB>
    {
        public TC Third { get; set; }

        public Magic()
            : this(default(TA))
        {

        }

        public Magic(TA first)
            : this(first, default(TB))
        {

        }

        public Magic(TA first, TB second)
            : this(first, second, default(TC))
        {

        }

        public Magic(TA first, TB second, TC third)
        {
            First = first;
            Second = second;
            Third = third;
        }
    }

    /// <summary>
    /// 魔法类
    /// </summary>
    /// <typeparam name="TA"></typeparam>
    /// <typeparam name="TB"></typeparam>
    /// <typeparam name="TC"></typeparam>
    /// <typeparam name="TD"></typeparam>
    public class Magic<TA, TB, TC, TD> : Magic<TA, TB, TC>
    {
        public TD Fourth { get; set; }
        public Magic()
            : this(default(TA))
        {

        }

        public Magic(TA first)
            : this(first, default(TB))
        {

        }

        public Magic(TA first, TB second)
            : this(first, second, default(TC))
        {

        }

        public Magic(TA first, TB second, TC third)
            : this(first, second, third, default(TD))
        {

        }
        public Magic(TA first, TB second, TC third, TD fourth)
        {
            First = first;
            Second = second;
            Third = third;
            Fourth = fourth;
        }
    }

    /// <summary>
    /// 魔法类
    /// </summary>
    /// <typeparam name="TA"></typeparam>
    /// <typeparam name="TB"></typeparam>
    /// <typeparam name="TC"></typeparam>
    /// <typeparam name="TD"></typeparam>
    /// <typeparam name="TE"></typeparam>
    public class Magic<TA, TB, TC, TD, TE> : Magic<TA, TB, TC, TD>
    {
        public TE Fifth { get; set; }
        public Magic()
            : this(default(TA))
        {

        }

        public Magic(TA first)
            : this(first, default(TB))
        {

        }

        public Magic(TA first, TB second)
            : this(first, second, default(TC))
        {

        }

        public Magic(TA first, TB second, TC third)
            : this(first, second, third, default(TD))
        {

        }
        public Magic(TA first, TB second, TC third, TD fourth)
            : this(first, second, third, fourth, default(TE))
        {

        }
        public Magic(TA first, TB second, TC third, TD fourth, TE fifth)
        {
            First = first;
            Second = second;
            Third = third;
            Fourth = fourth;
            Fifth = fifth;
        }
    }
}