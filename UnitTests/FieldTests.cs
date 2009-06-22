﻿using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using QuickFIX.NET;
using QuickFIX.NET.Fields;

namespace UnitTests
{
    [TestFixture]
    public class FieldTests
    {
        [Test]
        public void IntFieldTest()
        {
            IntField field = new IntField(Tags.AdvTransType, 500);
            Assert.That(field.getValue(), Is.EqualTo(500));
            Assert.That(field.Tag, Is.EqualTo(5));
            Assert.That(field.ToString(), Is.EqualTo("500"));
            Assert.That(field.Obj, Is.EqualTo(500));
            Assert.That(field.Tag, Is.EqualTo(5));
            field.Tag = 10;
            Assert.That(field.Tag, Is.EqualTo(10));
        }

        [Test]
        public void StringFieldTest()
        {
            StringField field = new StringField(200, "wayner");
            Assert.That(field.Obj, Is.EqualTo("wayner"));
            Assert.That(field.getValue(), Is.EqualTo("wayner"));
            Assert.That(field.Tag, Is.EqualTo(200));
            field.setValue("galway");
            Assert.That(field.Obj, Is.EqualTo("galway"));
        }

        [Test]
        public void CharFieldTest()
        {
            CharField field = new CharField(200, '3');
            Assert.That(field.Obj, Is.EqualTo('3'));
            Assert.That(field.getValue(), Is.EqualTo('3'));
            Assert.That(field.Tag, Is.EqualTo(200));
        }

        [Test]
        public void DecimalFieldTest()
        {
            Decimal val = new Decimal(3.232535);
            Decimal newval = new Decimal(3.14159);
            DecimalField field = new DecimalField(200, val);
            Assert.That(field.Obj, Is.EqualTo(val));
            Assert.That(field.getValue(), Is.EqualTo(val));
            Assert.That(field.Tag, Is.EqualTo(200));
            field.Obj = newval;
            Assert.That(field.Obj, Is.EqualTo(newval));
        }

        [Test]
        public void BooleanFieldTest()
        {
            BooleanField field = new BooleanField(10, true);
            Assert.That(field.Obj, Is.EqualTo(true));
            Assert.That(field.getValue(), Is.EqualTo(true));
            Assert.That(field.Tag, Is.EqualTo(10));
            field.Obj = false;
            Assert.That(field.Obj, Is.EqualTo(false));
        }

        [Test]
        public void DateTimeFieldTest()
        {
            DateTime val = new DateTime( 2009, 9, 4, 3, 44, 1 );
            DateTime newval = new DateTime(2009, 9, 4, 3, 44, 1);
            DateTimeField field = new DateTimeField(200, val);
            Assert.That(field.Obj, Is.EqualTo(val));
            Assert.That(field.getValue(), Is.EqualTo(val));
            Assert.That(field.Tag, Is.EqualTo(200));
            field.Obj = newval;
            Assert.That(field.Obj, Is.EqualTo(newval));
            Assert.That(field.ToString(), Is.EqualTo("20090904-03:44:01"));
        }


        [Test]
        public void StringFieldTest_TotalAndLength()
        {
            /// <remarks>
            /// from quickfix/j FieldTest.java
            /// </remarks>  
            StringField obj = new StringField(12, "VALUE");
            Assert.That(obj.toStringField(), Is.EqualTo("12=VALUE"));
            Assert.That(obj.getTotal(), Is.EqualTo(542));
            Assert.That(obj.getLength(), Is.EqualTo(9));
            obj.Obj = "VALUF"; // F = E+1
            Assert.That(obj.toStringField(), Is.EqualTo("12=VALUF"));
            Assert.That(obj.getTotal(), Is.EqualTo(543));
            Assert.That(obj.getLength(), Is.EqualTo(9));
            obj.Tag = 13; // 13 = 12+1
            Assert.That(obj.toStringField(), Is.EqualTo("13=VALUF"));
            Assert.That(obj.getTotal(), Is.EqualTo(544));
            Assert.That(obj.getLength(), Is.EqualTo(9));
        }

        [Test]
        public void DefaultValTest()
        {
            BooleanField bf = new BooleanField(110);
            Assert.That(false, Is.EqualTo(bf.Obj));
            Assert.That(110, Is.EqualTo(bf.Tag));
            CharField cf = new CharField(300);
            Assert.That('\0', Is.EqualTo(cf.getValue()));
            Assert.That(300, Is.EqualTo(cf.Tag));
            DateTimeField dtf = new DateTimeField(3);
            Assert.That(3, Is.EqualTo(dtf.Tag));
            StringField sf = new StringField(32);
            Assert.That(32, Is.EqualTo(sf.Tag));
            Assert.That("", Is.EqualTo(sf.Obj));
            IntField ifld = new IntField(239);
            Assert.That(239, Is.EqualTo(ifld.Tag));
            Assert.That(0, Is.EqualTo(ifld.Obj));
            DecimalField df = new DecimalField(1);
            Assert.That(1, Is.EqualTo(df.Tag));
            Assert.That(new Decimal(0.0), Is.EqualTo(df.Obj));
        }
    }
}