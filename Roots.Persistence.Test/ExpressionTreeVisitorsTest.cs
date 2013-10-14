using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq.Expressions;
using System.Linq;
using Roots.Persistence.Cache;
using SharpTestsEx;
using System.Collections.Generic;

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

            replaced.NodeType.Should().Be(ExpressionType.Lambda);
            replaced.Parameters.Should().Have.Count.EqualTo(1);

            ((MethodCallExpression)((LambdaExpression)replaced.Body).Body).NodeType.Should().Be.EqualTo(ExpressionType.Call);
            ((MethodCallExpression)((LambdaExpression)replaced.Body).Body).Method.Name.Should().Be.EqualTo("Select");
            ((MethodCallExpression)((LambdaExpression)replaced.Body).Body).Arguments.Should().Have.Count.EqualTo(2);
            ((MethodCallExpression)((MethodCallExpression)((LambdaExpression)replaced.Body).Body).Arguments[0]).Method.Name.Should().Be.EqualTo("Where");
            ((MethodCallExpression)((MethodCallExpression)((LambdaExpression)replaced.Body).Body).Arguments[0]).Arguments.Should().Have.Count.EqualTo(2);
            ((MethodCallExpression)((MethodCallExpression)((LambdaExpression)replaced.Body).Body).Arguments[0]).Arguments[0].Should().Be.EqualTo(replaced.Parameters[0]);

        }

        [TestMethod]
        public void ReplaceConstantWithNewConstant()
        {

            Expression<Func<IQueryable>> exp = () => repo.Where(x => x.Value == 1).Select(x => x);

            var replacer = new ChangeConstant<IQueryable<TestClass>>();

            var newConstant =  new List<TestClass>().AsQueryable();

            var replaced = (Expression<Func<IQueryable>>)replacer.Replace(exp,newConstant);

            ((ConstantExpression)((MethodCallExpression)((MethodCallExpression)((LambdaExpression)replaced).Body).Arguments[0]).Arguments[0]).Value.Should().Be.EqualTo(newConstant);

        }

    }
}
