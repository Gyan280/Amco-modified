using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace amco
{
    public partial class PrintReceipt : Form
    {
        List<Receipt> _list;
        string _total, _cash, _change, _date, _companyName,_subtotal,_invoiceno, _vat,_nhis;

        private void reportViewer_Load_1(object sender, EventArgs e)
        {

        }

        private void reportViewer_Load(object sender, EventArgs e)
        {

        }

        //You can pass data by using constructor
        public PrintReceipt(List<Receipt> dataSource, string total, string cash, string change, string date, string companyName,string subtotal, string invoiceno ,string vat, string nhis)
        {
            InitializeComponent();
            _list = dataSource;
            _total = total;
            _cash = cash;
            _change = change;
            _date = date;
            _companyName = companyName;
            _subtotal = subtotal;
            _invoiceno = invoiceno;
            _vat = vat;
            _nhis = nhis;
        }
        private void PrintReceipt_Load(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            //Set datasource, parameters to RDLC report
            ReceiptBindingSource.DataSource = _list ;
            Microsoft.Reporting.WinForms.ReportParameter[] para = new Microsoft.Reporting.WinForms.ReportParameter[]
            {

                new Microsoft.Reporting.WinForms.ReportParameter("pTotal",_total),
                new Microsoft.Reporting.WinForms.ReportParameter("pCash",_cash),
                new Microsoft.Reporting.WinForms.ReportParameter("pChange",_change),
                new Microsoft.Reporting.WinForms.ReportParameter("pDate",_date),
                new Microsoft.Reporting.WinForms.ReportParameter("pCompanyName",_companyName),
                new Microsoft.Reporting.WinForms.ReportParameter("pSubtotal",_subtotal),
                new Microsoft.Reporting.WinForms.ReportParameter("pInvoiceNo",_invoiceno),
                new Microsoft.Reporting.WinForms.ReportParameter("pVAT",_vat),
                 new Microsoft.Reporting.WinForms.ReportParameter("pNHIS", _nhis)
            };
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataSet1";
            reportDataSource.Value = _list;
            this.reportViewer.LocalReport.DataSources.Add(reportDataSource);
            this.reportViewer.LocalReport.SetParameters(para);
            this.reportViewer.RefreshReport();
        }


    }
}
