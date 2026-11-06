using Microsoft.AspNetCore.Hosting;
using MimeKit;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using Tabeeb.Application.Common.Services;
using Tabeeb.Domain.Common;

namespace Tabeeb.Infrastructure.Services
{
    public class InvoiceGenerator (IWebHostEnvironment webHostEnvironment): IInvoiceGenerator, IDocument
    {
        private SubscriptionInvoiceDto _currentInvoice = null!;

        // 1. تنفيذ دالة التوليد المطلوبة من واجهة الخدمة IInvoiceGenerator
        public byte[] GenerateSubscriptionInvoice(SubscriptionInvoiceDto subscriptionInvoice)
        {
            // حفظ البيانات الحالية لاستخدامها داخل هيكل المستند
            _currentInvoice = subscriptionInvoice;

            // ضبط ترخيص QuestPDF (يُفضل وضعه في Program.cs أو هنا كاحتياط)
            QuestPDF.Settings.License = LicenseType.Community;

            // توليد ملف الـ PDF وإرجاعه على هيئة مصفوفة بايتبايت
            return this.GeneratePdf();
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        // 2. هيكل المستند الأساسي لـ QuestPDF (تنفيذ واجهة IDocument)
        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                // إعداد الاتجاه إلى اليمين واللغة العربية
                page.ContentFromRightToLeft();
                page.Size(PageSizes.A4);
                page.Margin(30);
                page.PageColor(Colors.White);

                page.DefaultTextStyle(x => x.FontFamily("Cairo").FontSize(10).FontColor("#334155"));
                // تطبيق خلفية الـ Pattern المبتكرة (النسخة الآمنة بدون SkiaSharp)
                page.Background().Element(ComposeBackgroundPattern);

                // الهيكل العام للفاتورة مع تمرير البيانات بشكل صحيح عبر Lambda
                page.Header().Element(headerContainer => ComposeHeader(headerContainer, _currentInvoice,webHostEnvironment));
                if (_currentInvoice.Type != Domain.Enums.SubscriptionInvoiceType.Cancel)
                {
                    page.Content().Element(ComposeContent);

                }
                else
                {
                    page.Content().Element(ComposeCancelContent);
                }
                page.Footer().Element(ComposeFooter);
            });
        }

        #region Background Pattern
        private void ComposeBackgroundPattern(IContainer container)
        {
            container.Layers(layers =>
            {
                layers.PrimaryLayer();
                // عنصر خلفي في الزاوية العليا
                layers.Layer().AlignTop().AlignRight().PaddingRight(-50).PaddingTop(-50)
                    .Width(250).Height(250)
                    .Background("#F8FAFC")
                    .CornerRadius(125);

                // عنصر خلفي في الزاوية السفلى
                layers.Layer().AlignBottom().AlignLeft().PaddingLeft(-60).PaddingBottom(-60)
                    .Width(300).Height(300)
                    .Background("#F1F5F9")
                    .CornerRadius(150);
            });
        }
        #endregion

        #region Header
        private void ComposeHeader(IContainer container, SubscriptionInvoiceDto subscriptionInvoice,IWebHostEnvironment webHost)
        {
            var logoBytes = GetLogoBytesAsync(webHost);
            container.PaddingBottom(20).Row(row =>
            {
                // القسم الأيمن: شعار العيادة واسمها
                row.RelativeItem(2).Column(column =>
                {
                     
                   column.Item().Height(60).Width(140).Image(logoBytes, ImageScaling.FitArea);
                    

                    column.Item().PaddingTop(6).Text(subscriptionInvoice.ClinicName)
                          .FontSize(18)
                          .Bold()
                          .FontColor("#1E293B");

                    if (subscriptionInvoice.Type == Domain.Enums.SubscriptionInvoiceType.New)
                    {
                        column.Item().Text("فاتورة اشتراك خدمات نظام طبيب")
                         .FontSize(10)
                         .FontColor("#64748B");
                    }else if (subscriptionInvoice.Type == Domain.Enums.SubscriptionInvoiceType.Renew)
                    {
                        column.Item().Text("فاتورة تجديد اشتراك خدمات نظام طبيب")
                         .FontSize(10)
                         .FontColor("#64748B");
                    }
                    else
                    {
                        column.Item().Text("فاتورة الغاء اشتراك خدمات نظام طبيب")
                         .FontSize(10)
                         .FontColor("#64748B");
                    }
                   
                });

                // القسم الأيسر: تفاصيل الفاتورة والـ QR Code
                row.RelativeItem(1).AlignLeft().Column(column =>
                {
                    column.Item().Text("فاتورة مبيعات")
                          .FontSize(20)
                          .Bold()
                          .FontColor("#0F172A");

                    column.Item().Text($"رقم الفاتورة: #{subscriptionInvoice.InvoiceNumber}")
                          .FontSize(10)
                          .FontColor("#475569");

                    column.Item().Text($"تاريخ الإصدار: {subscriptionInvoice.IssueDate:yyyy/MM/dd}")
                          .FontSize(10)
                          .FontColor("#475569");

                    if (subscriptionInvoice.QrCodeBytes != null && subscriptionInvoice.QrCodeBytes.Length > 0)
                    {
                        column.Item().PaddingTop(8).Width(75).Height(75).Image(subscriptionInvoice.QrCodeBytes);
                    }
                });
            });
        }
        #endregion

        #region Content
        private void ComposeContent(IContainer container)
        {
            container.PaddingVertical(10).Column(column =>
            {
                // جدول تفاصيل الاشتراك والخدمة
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(40);  // م
                        columns.RelativeColumn(3);  // الخدمة / الباقة
                        columns.RelativeColumn(1);  // السعر
                    });

                    // ترويسة الجدول (Header)
                    table.Header(header =>
                    {
                        header.Cell().Element(CellStyleHeader).Text("#").Bold();
                        header.Cell().Element(CellStyleHeader).Text("اسم باقة الاشتراك / الخدمة").Bold();
                        header.Cell().Element(CellStyleHeader).AlignLeft().Text("المبلغ الإجمالي").Bold();

                        static IContainer CellStyleHeader(IContainer container)
                        {
                            return container
                                .Background("#0F172A")
                                .Padding(8)
                                .DefaultTextStyle(x => x.FontColor(Colors.White).FontSize(10));
                        }
                    });

                    // صف البيانات (Row) باستخدام البيانات المخزنة
                    table.Cell().Element(CellStyleRow).Text("1");
                    table.Cell().Element(CellStyleRow).Text($"اشتراك باقة ({_currentInvoice.PackageName}) - نظام طبيب الإداري");
                    table.Cell().Element(CellStyleRow).AlignLeft().Text($"{_currentInvoice.TotalAmount:N2} ر.س");

                    static IContainer CellStyleRow(IContainer container)
                    {
                        return container
                            .BorderBottom(1)
                            .BorderColor("#E2E8F0")
                            .Padding(10)
                            .DefaultTextStyle(x => x.FontSize(10).FontColor("#334155"));
                    }
                });

                column.Item().PaddingTop(25);

                // ملخص المبالغ المالية (Total Breakdown)
                column.Item().Row(row =>
                {
                    row.RelativeItem(1).Column(col =>
                    {
                        col.Item().Background("#F8FAFC")
                           .Border(1)
                           .BorderColor("#E2E8F0")
                           .Padding(12)
                           .Column(innerCol =>
                           {
                               innerCol.Item().Text("ملاحظات وشروط Payment Terms:")
                                       .FontSize(9).Bold().FontColor("#475569");
                               innerCol.Item().Text("تعتبر هذه الفاتورة مستنداً رسمياً بسداد قيمة الاشتراك الموضحة أعلى.")
                                       .FontSize(8).FontColor("#64748B");
                           });
                    });

                    row.ConstantItem(20); // مسافة فاصلة

                    row.RelativeItem(1).Column(col =>
                    {
                        col.Item().Row(r =>
                        {
                            r.RelativeItem().Text("الإجمالي الكلي:").FontSize(10).Bold();
                            r.RelativeItem().AlignLeft().Text($"{_currentInvoice.TotalAmount:N2} ر.س").FontSize(10).Bold();
                        });

                        col.Item().PaddingTop(4).Row(r =>
                        {
                            r.RelativeItem().Text("المبلغ المدفوع:").FontSize(10).FontColor("#16A34A");
                            r.RelativeItem().AlignLeft().Text($"{_currentInvoice.PaidAmount:N2} ر.س").FontSize(10).FontColor("#16A34A").Bold();
                        });

                        col.Item().PaddingTop(4).BorderBottom(1).BorderColor("#CBD5E1").PaddingBottom(6).Row(r =>
                        {
                            r.RelativeItem().Text("المبلغ المتبقي:").FontSize(10).FontColor("#DC2626");
                            r.RelativeItem().AlignLeft().Text($"{_currentInvoice.RemainingAmount:N2} ر.س").FontSize(10).FontColor("#DC2626").Bold();
                        });
                    });
                });
            });
        }
        #endregion
        private void ComposeCancelContent(IContainer container)
        {
            int days = DateOnly.FromDateTime(DateTime.UtcNow).DayNumber - _currentInvoice.StartAt.DayNumber;

            var dayPrice = _currentInvoice.DaysCount!=0 ?(decimal)_currentInvoice.TotalAmount / _currentInvoice.DaysCount :0;
            var daysprice = (dayPrice * days);
            container.PaddingVertical(10).Column(column =>
            {
                // جدول تفاصيل الاشتراك والخدمة
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(40);  // م
                        columns.RelativeColumn(3);  // الخدمة / الباقة
                        columns.RelativeColumn(1);  // السعر
                        columns.RelativeColumn(1);
                        columns.RelativeColumn(1);
                        columns.RelativeColumn(1);
                    });

                    // ترويسة الجدول (Header)
                    table.Header(header =>
                    {
                        header.Cell().Element(CellStyleHeader).Text("#").Bold();
                        header.Cell().Element(CellStyleHeader).Text("اسم باقة الاشتراك / الخدمة").Bold();
                        header.Cell().Element(CellStyleHeader).AlignLeft().Text("المبلغ الإجمالي").Bold();
                        header.Cell().Element(CellStyleHeader).AlignLeft().Text("عدد الايام").Bold();
                        header.Cell().Element(CellStyleHeader).AlignLeft().Text("عدد الايام المستهلكة").Bold();
                        header.Cell().Element(CellStyleHeader).AlignLeft().Text("الرصيد المستهلك").Bold();



                        static IContainer CellStyleHeader(IContainer container)
                        {
                            return container
                                .Background("#0F172A")
                                .Padding(8)
                                .DefaultTextStyle(x => x.FontColor(Colors.White).FontSize(10));
                        }
                    });

                
                    // صف البيانات (Row) باستخدام البيانات المخزنة
                    table.Cell().Element(CellStyleRow).Text("1");
                    table.Cell().Element(CellStyleRow).Text($" الغاء اشتراك باقة  ({_currentInvoice.PackageName}) - نظام طبيب الإداري");
                    table.Cell().Element(CellStyleRow).AlignLeft().Text($"{_currentInvoice.TotalAmount:N2}");
                    table.Cell().Element(CellStyleRow).AlignLeft().Text($"{_currentInvoice.DaysCount}");
                    table.Cell().Element(CellStyleRow).AlignLeft().Text($"{days}");
                    table.Cell().Element(CellStyleRow).AlignLeft().Text($"{daysprice:N2}");



                    static IContainer CellStyleRow(IContainer container)
                    {
                        return container
                            .BorderBottom(1)
                            .BorderColor("#E2E8F0")
                            .Padding(10)
                            .DefaultTextStyle(x => x.FontSize(10).FontColor("#334155"));
                    }
                });

                column.Item().PaddingTop(25);

                // ملخص المبالغ المالية (Total Breakdown)
                column.Item().Row(row =>
                {
                    row.RelativeItem(1).Column(col =>
                    {
                        col.Item().Background("#F8FAFC")
                           .Border(1)
                           .BorderColor("#E2E8F0")
                           .Padding(12)
                           .Column(innerCol =>
                           {
                               innerCol.Item().Text("ملاحظات وشروط Payment Terms:")
                                       .FontSize(9).Bold().FontColor("#475569");
                               innerCol.Item().Text("تعتبر هذه الفاتورة مستنداً رسمياً بسداد قيمة الاشتراك الموضحة أعلى.")
                                       .FontSize(8).FontColor("#64748B");
                           });
                    });

                    row.ConstantItem(20); // مسافة فاصلة

                        row.RelativeItem(1).Column(col =>
                        {
                            col.Item().Row(r =>
                            {
                                r.RelativeItem().Text("الإجمالي الكلي:").FontSize(10).Bold();
                                r.RelativeItem().AlignLeft().Text($"{_currentInvoice.TotalAmount:N2} ر.س").FontSize(10).Bold();
                            });

                            col.Item().PaddingTop(4).Row(r =>
                            {
                                r.RelativeItem().Text("المبلغ المستهلك:").FontSize(10).FontColor("#16A34A");
                                r.RelativeItem().AlignLeft().Text($"{daysprice:N2} ر.س").FontSize(10).FontColor("#16A34A").Bold();
                            });

                            col.Item().PaddingTop(4).BorderBottom(1).BorderColor("#CBD5E1").PaddingBottom(6).Row(r =>
                            {
                                r.RelativeItem().Text("المبلغ المتبقي:").FontSize(10).FontColor("#DC2626");
                                r.RelativeItem().AlignLeft().Text($"{(_currentInvoice.TotalAmount - daysprice):N2} ر.س").FontSize(10).FontColor("#DC2626").Bold();
                            });
                        });

                    
                    

                });
            });
        }

        #region Footer
        private void ComposeFooter(IContainer container)
        {
            container.Column(col =>
            {
                col.Item().BorderTop(1).BorderColor("#E2E8F0").PaddingTop(10).Row(row =>
                {
                    row.RelativeItem().Text(text =>
                    {
                        text.Span("تم إنشاء هذه الفاتورة تلقائياً بواسطة ").FontSize(9).FontColor("#64748B");
                        text.Span("نظام طبيب الإداري (Tabeeb System)").Bold().FontSize(9).FontColor("#0F172A");
                    });

                    row.RelativeItem().AlignLeft().Text(x =>
                    {
                        x.Span("صفحة ").FontSize(8).FontColor("#94A3B8");
                        x.CurrentPageNumber().FontSize(8).FontColor("#94A3B8");
                        x.Span(" من ").FontSize(8).FontColor("#94A3B8");
                        x.TotalPages().FontSize(8).FontColor("#94A3B8");
                    });
                });
            });
        }
        #endregion


        public  byte[]? GetLogoBytesAsync(IWebHostEnvironment _webHostEnvironment )
        {
            // تحديد مسار اللوجو داخل wwwroot (مثلاً wwwroot/images/logo.png)
            string logoPath = Path.Combine(_webHostEnvironment.WebRootPath, "Logo.jpg");

            if (File.Exists(logoPath))
            {
                return  File.ReadAllBytes(logoPath);
            }

            return null; // لو الملف مش موجود
        }
    }
}