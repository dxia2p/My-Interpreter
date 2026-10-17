using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolInterpreter {
    abstract internal class Expr {

    }

    internal class BinaryExpr : Expr {
        BinaryExpr(Expr left, Token op, Expr right) {
            this.left = left;
            this.op = op;
            this.right = right;
        }

        readonly Expr left;
        readonly Token op;
        readonly Expr right;
    }

    internal class UnaryExpr : Expr {
        UnaryExpr(Token op, Expr right) {
            this.op = op;
            this.right = right;
        }

        readonly Token op;
        readonly Expr right;
    }

    internal class LiteralExpr : Expr {
        LiteralExpr(Token value) {
            this.value = value;
        }

        readonly Token value;
    }

    internal class GroupingExpr : Expr {
        GroupingExpr(Expr expression) {
            this.expression = expression;
        }
        readonly Expr expression;
    }
}
