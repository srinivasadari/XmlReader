﻿using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlReader.FileWatcher.Extensions;
using XmlReader.FileWatcher.Models;
using XmlReader.FileWatcher.XmlFileHandling;

namespace XmlReader.FileWatcher.Tests
{
    [TestFixture]
    public class FileContentHandlerTests
    {
        List<string> validMeterReading;

        [SetUp]
        public void Setup()
        {
            validMeterReading = new List<string>() {
                "100,NEM12,201801211010,MYENRGY,URENRGY",
                "200,12345678901,E1,E1,E1,N1,HGLMET501,KWH,30,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "200,98765432109,E1,E1,E1,N1,HGLMET502,KWH,30,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "900"
            };
        }
        [Test]
        public void ValidateCsvInterValBlockData_AllareValid_ShouldReturnTrue()
        {
            var xmlContentReaderMock = new Mock<IXmlContentReader>();
            FileContentHandler xd = new FileContentHandler(xmlContentReaderMock.Object);
            var result = xd.ValidateCsvInterValBlockData(validMeterReading);
            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void ValidateCsvInterValBlockData_InvalidHeader_ShouldReturnFalse()
        {
            List<string> csvBlocKData = new List<string>()
            {
                "10,NEM12,201801211010,MYENRGY,URENRGY",
                "200,12345678901,E1,E1,E1,N1,HGLMET501,KWH,30,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "200,98765432109,E1,E1,E1,N1,HGLMET502,KWH,30,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "900"
            };

            var xmlContentReaderMock = new Mock<IXmlContentReader>();
            FileContentHandler xd = new FileContentHandler(xmlContentReaderMock.Object);
            var result = xd.ValidateCsvInterValBlockData(csvBlocKData);
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void ValidateCsvInterValBlockData_InvalidFooter_ShouldReturnFalse()
        {
            List<string> csvBlocKData = new List<string>()
            {
                "100,NEM12,201801211010,MYENRGY,URENRGY",
                "200,12345678901,E1,E1,E1,N1,HGLMET501,KWH,30,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "200,98765432109,E1,E1,E1,N1,HGLMET502,KWH,30,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300"
            };

            var xmlContentReaderMock = new Mock<IXmlContentReader>();
            FileContentHandler xd = new FileContentHandler(xmlContentReaderMock.Object);
            var result = xd.ValidateCsvInterValBlockData(csvBlocKData);
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void ValidateCsvInterValBlockData_NotContain200_ShouldReturnFalse()
        {
            List<string> csvBlocKData = new List<string>()
            {
                "100,NEM12,201801211010,MYENRGY,URENRGY",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "900"
            };

            var xmlContentReaderMock = new Mock<IXmlContentReader>();
            FileContentHandler xd = new FileContentHandler(xmlContentReaderMock.Object);
            var result = xd.ValidateCsvInterValBlockData(csvBlocKData);
            Assert.That(result, Is.EqualTo(false));
        }
        [Test]
        public void ValidateCsvInterValBlockData_ContainSpaces_ShouldReturnTrue()
        {
            List<string> csvBlocKData = new List<string>()
            {
                "100, NEM12 ,201801211010,   MYENRGY,URENRGY    ,    ,     ",
                "200,12345678901,E1,E1,E1,N1,HGLMET501,KWH,30,",
                "300,20180115,5.000,  3.000  ,3.008,3.000,4.000,3.000,2.96,3.6,,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,  3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "200,12345678901,E1,E1,E1,N1,HGLMET501,KWH,30,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,  3.000  ,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "900"
            };

            var xmlContentReaderMock = new Mock<IXmlContentReader>();
            FileContentHandler xd = new FileContentHandler(xmlContentReaderMock.Object);
            var result = xd.ValidateCsvInterValBlockData(csvBlocKData);
            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void ValidateCsvInterValBlockData_NoThreeHundredRecords_ShouldReturnFalse()
        {
            List<string> csvBlocKData = new List<string>()
            {
                "100, NEM12 ,201801211010,   MYENRGY,URENRGY    ,    ,     ",
                "200,12345678901,E1,E1,E1,N1,HGLMET501,KWH,30,",
                "3000,20180115,5.000,  3.000  ,3.008,3.000,4.000,3.000,2.96,3.6,,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "3000,20180115,5.000,  3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "3000,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "3000,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "3000,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "200,12345678901,E1,E1,E1,N1,HGLMET501,KWH,30,",
                "3000,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "3000,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "3000,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "3000,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "3000,20180115,5.000,  3.000  ,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "900"
            };

            var xmlContentReaderMock = new Mock<IXmlContentReader>();
            FileContentHandler xd = new FileContentHandler(xmlContentReaderMock.Object);
            var result = xd.ValidateCsvInterValBlockData(csvBlocKData);
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void ValidateCsvInterValBlockData_200rowmustbefollowedbyatleast300_ShouldReturnFalse()
        {
            List<string> csvBlocKData = new List<string>()
            {
                "100, NEM12 ,201801211010,MYENRGY,URENRGY ",
                "200,12345678901,E1,E1,E1,N1,HGLMET501,KWH,30,",
                "3000,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "200,12345678901,E1,E1,E1,N1,HGLMET501,KWH,30,",
                "3000,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "300,20180115,5.000,  3.000  ,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,",
                "900"
            };

            var xmlContentReaderMock = new Mock<IXmlContentReader>();
            FileContentHandler xd = new FileContentHandler(xmlContentReaderMock.Object);
            var result = xd.ValidateCsvInterValBlockData(csvBlocKData);
            Assert.That(result, Is.EqualTo(false));
        }
    }
}
