﻿using System;
using NUnit.Framework;
using FunctionalExtensions.Lambda;

namespace FunctionalExtensions.Tests
{
    [TestFixture]
    public class OptionTests
    {
        [Test]
        public void ValidationWithOptionMonad_HappyDay_Test()
        {
            var result = 0.0m;
            var d1 = Fun.Create(() => Option.Some(2.5m));
            var d2 = Fun.Create(() => Option.Some(2.5m));

            var callbackSome = Act.Create((decimal x) => result = x);

            var callbackNone = Act.Create(Assert.Fail);

            ValidationWithOptionMonad(d1, d2, Division.Divide, callbackSome, callbackNone);
            Assert.That(result, Is.EqualTo(100));
        }

        [Test]
        public void ValidationWithOptionMonad_RainyDay_Test()
        {
            var isCalled = false;
            var d1 = Fun.Create(() => Option.Some(2.5m));
            var zero = Fun.Create(() => Option.Some(0.0m));
            var ex = Fun.Create(Option.None<decimal>);

            Action callbackNone = () => isCalled = true;
            var callbackSome = Act.Create((decimal x) => Assert.Fail());

            isCalled = false;
            ValidationWithOptionMonad(d1, ex, Division.Divide, callbackSome, callbackNone);
            Assert.That(isCalled, Is.EqualTo(true));

            isCalled = false;
            ValidationWithOptionMonad(ex, d1, Division.Divide, callbackSome, callbackNone);
            Assert.That(isCalled, Is.EqualTo(true));

            isCalled = false;
            ValidationWithOptionMonad(d1, zero, Division.Divide, callbackSome, callbackNone);
            Assert.That(isCalled, Is.EqualTo(true));
        }

        private static void ValidationWithOptionMonad(Func<Option<decimal>> readdecimal1, Func<Option<decimal>> readdecimal2, Func<decimal, decimal, Option<decimal>> divide, Action<decimal> callBackSome, Action callBackNone)
        {
            (
                from v1 in readdecimal1()
                from v2 in readdecimal2()
                from result in divide(v1, v2)
                select result*100
            )
                .Match(callBackSome, callBackNone);
        }

        [Test]
        public void ToOption_Test()
        {
            var option1 = 5.ToOption();

            option1.Match(
                x => Assert.That(x, Is.EqualTo(5)),
                () => Assert.Fail());

            String s = null;

            var option2 = s.ToOption();

            option2.Match(
                x => Assert.Fail(),
                () => Assert.Pass());
        }

        [Test]
        public void Equals_Test()
        {
            Assert.That(42.ToOption(), Is.EqualTo(42.ToOption()));
            Assert.That(Option.None<int>(), Is.EqualTo(Option.None<int>()));
            Assert.That(Option.Some(42).Equals(Option.Some(42)));
            Assert.That((Option.Some(42).Equals(42)));
            Assert.That(!Option.None<int>().Equals(Option.None<string>()));
            Assert.That(Option.None<int>().Equals(null));
        }

        [Test]
        public void Monad_Test()
        {
            var k = Fun.Create((int x) => Option.Some(x * 2));

            // Left Identity
            Assert.That(Option.Some(42).Bind(k), Is.EqualTo(k(42)));

            // Right Identity
            var m = Option.Some(42);
            Assert.That(m.Bind(x => x.ToOption()), Is.EqualTo(m));

            // Associative
            var h = Fun.Create((int x) => Option.Some(x + 1));
            Assert.That(
                m.Bind(x => k(x).Bind(h)),
                Is.EqualTo(m.Bind(k).Bind(h)));
        }
    }
}
