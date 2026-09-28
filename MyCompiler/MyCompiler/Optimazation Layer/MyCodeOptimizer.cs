using System;
using System.Collections.Generic;

namespace MyCompiler.Optimazation_Layer
{
    public class MyCodeOptimizer
    {
        private readonly MyConstantFoldingOptimizer constantFolding;
        private readonly MyAlgebraicOptimizer algebraic;
        private readonly MyCopyPropagationOptimizer copyPropagation;
        private readonly MyDeadCodeOptimizer deadCode;

        public MyCodeOptimizer()
        {
            constantFolding = new MyConstantFoldingOptimizer();

            algebraic = new MyAlgebraicOptimizer();

            copyPropagation = new MyCopyPropagationOptimizer();

            deadCode = new MyDeadCodeOptimizer();
        }

        public List<string> Optimize(List<string> instructions)
        {
            if (instructions == null)
                return new List<string>();

            List<string> result = new List<string>(instructions);

            // 1. Constant Folding
            result = constantFolding.Optimize(result);

            // 2. Algebraic Simplification
            result = algebraic.Optimize(result);

            // 3. Copy / Constant Propagation
            result = copyPropagation.Optimize(result);

            // 4. Constant Folding مرة ثانية
            // لأن Propagation قد ينتج تعبيراً
            // يحتوي على قيم ثابتة
            result = constantFolding.Optimize(result);

            // 5. Algebraic Simplification مرة ثانية
            result = algebraic.Optimize(result);

            // 6. إزالة الـ temporaries غير المستخدمة
            result = deadCode.Optimize(result);

            return result;
        }

        public string Optimize(string intermediateCode)
        {
            if (string.IsNullOrWhiteSpace(intermediateCode))
                return string.Empty;

            List<string> instructions =
                new List<string>(
                    intermediateCode.Split(
                        new[] { "\r\n", "\n" },
                        StringSplitOptions.RemoveEmptyEntries
                    )
                );

            List<string> optimized =
                Optimize(instructions);

            return string.Join(
                Environment.NewLine,
                optimized
            );
        }
    }
}