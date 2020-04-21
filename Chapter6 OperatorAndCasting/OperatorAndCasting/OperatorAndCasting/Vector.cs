namespace OperatorAndCasting
{
    public struct Vector
    {
        public Vector(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector(Vector v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
        }
        public double X { get; }
        public double Y { get; }
        public double Z { get; }
        public override string ToString() => $"( {X}, {Y}, {Z})";
        //为+运算符提供支持的运算符重载
        public static Vector operator +(Vector left, Vector right) =>
            new Vector(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        //为*运算符提供重载, 乘号*左边是double右边是Vector
        public static Vector operator *(double left, Vector right) =>
            new Vector(left * right.X, left * right.Y, left * right.Z);
        //为*运算符提供重载, 乘号*左边是Vector右边是double
        public static Vector operator *(Vector left, double right) =>
        //下面是标准写法
        // new Vector(right * left.X, right * left.Y, right * left.Z);
        //可以直接重用上面左边double右边Vector的代码
        new Vector(right * left);
        //计算两个向量的内积（或点积）
        public static double operator *(Vector left, Vector right) =>
           left.X * right.X + left.Y * right.Y + left.Z * right.Z;

        //重载==比较运算符
        public static bool operator ==(Vector left, Vector right)
        {
            if (object.ReferenceEquals(left, right)) return true;
            return left.X == right.X && left.Y == right.Y && left.Z == right.Z;
        }
        //如果重载==比较符，一定要成对重载!==比较符
        public static bool operator !=(Vector left, Vector right) =>
            !(left == right);

        //Equals和GetHashCode方法应该总是在重写==运算符时进行重写，否则编译器会报错
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return this == (Vector)obj;
        }
        public override int GetHashCode() =>
            X.GetHashCode() ^ (Y.GetHashCode()) ^ Z.GetHashCode();
    }
}