using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq.Expressions;
using System.Linq;
using Roots.Persistence.Cache;
using SharpTestsEx;

namespace Roots.Persistence.Test
{
    [TestClass]
    public class ExpressionTreeVisitorsTest
    {
        class TestClass
        {
            public int Value { get; set; }
        }

        [TestMethod]
        public void ExpressionTreeDisassembler()
        {
            MemoryRepository<TestClass> repo = new MemoryRepository<TestClass>(null);

            Expression<Func<IQueryable>> exp = () => repo.Where(x => x.Value == 1).Select(x => x.Value);

            var disassembler = new TypeDisassembler<IQueryable<TestClass>>();
            disassembler.Disassemble(exp, repo);

            disassembler.FirstPart.Should().Not.Be.Null();
            disassembler.FirstPart.NodeType.Should().Be.EqualTo(ExpressionType.Lambda);

            disassembler.SecondPart.Should().Not.Be.Null();
            disassembler.SecondPart.NodeType.Should().Be.EqualTo(ExpressionType.Call);
            ((MethodCallExpression)disassembler.SecondPart).Method.Name.Should().Be.EqualTo("Where");
            
        }


        [TestMethod]
        public void ExpressionTreeConstFinder()
        {
            MemoryRepository<TestClass> repo = new MemoryRepository<TestClass>(null);

            Expression<Func<IQueryable>> exp = () => repo.Where(x => x.Value == 1).Select(x => x);

            var disassembler = new ConstFinder<IQueryable<TestClass>>();
            var value = disassembler.Find(exp);

            value.Should().Not.Be.Null();
            value.Should().Be.SameInstanceAs(repo);
        }

        MemoryRepository<TestClass> repo = new MemoryRepository<TestClass>(null);

        [TestMethod]
        public void ExpressionTreeConstFinderOnReference()
        {
            

            Expression<Func<IQueryable>> exp = () => repo.Where(x => x.Value == 1).Select(x => x);

            var disassembler = new ConstFinder<IQueryable<TestClass>>();
            var value = disassembler.Find(exp);

            value.Should().Not.Be.Null();
            value.Should().Be.SameInstanceAs(repo);
        }


        [TestMethod]
        public void ReplaceConstantWithParameterAndMakeLambda()
        {


            Expression<Func<IQueryable>> exp = () => repo.Where(x => x.Value == 1).Select(x => x);

            var replacer = new MakeLambdaOnParameter<IQueryable<TestClass>>();
            var replaced = replacer.Replace(exp);
        }

    }
}
