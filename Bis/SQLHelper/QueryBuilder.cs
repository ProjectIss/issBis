using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bis.SQLHelper
{
    public class QueryBuilder
    {
        public string BuildQuery(string reportName)
        {
            string query = string.Empty;
            switch (reportName)
            {
                case "AttendanceByCategory":
                    query = "select * from Employee where Employee.categoryId = (select top 1 id from Category where Category.name =@Category)";
                    break;
                case "AttendanceConsolidate":
                    query = "ReportForAttendance";
                    break;
                case "AdvanceReport":
                    query = "SELECT emp.employeeid,emp.name AS 'Name',ctg.name AS 'Category',FORMAT(adv.date,'dd-MM-yyyy') AS 'Date',SUM(adv.amount) AS 'Advance'" +
                        "FROM Advance adv" + " " +
                        "INNER JOIN Employee emp on adv.employeeId = emp.id" + " " +
                        "inner join Category ctg on ctg.id = emp.categoryId" + " " +
                        "where ctg.id = @ID AND adv.employeeId = @EMPLOYEEID AND adv.date BETWEEN @DATEFROM AND @DATETO " + " " +
                        "group By adv.employeeId,emp.employeeid,emp.id,emp.name,ctg.name,adv.date" + " " +
                        "Order by Date, employeeId; ";
                    break;
                case "LoanReport":
                    query = "SELECT ROW_NUMBER() OVER(ORDER BY T1.EMPLOYEEID ASC) AS SNo,T1.employeeId,T1.name,T2.Loan,T3.Deduction,(T2.Loan-T3.Deduction) AS Balance  FROM " +
                            " (SELECT emp.id, emp.employeeid, emp.name " +
                            " FROM EMPLOYEE emp " +
                            " WHERE emp.categoryid = @ID) AS T1 " +
                            " INNER JOIN( " +
                            " SELECT EMPLOYEEID, SUM(LOANAMOUNT) AS 'Loan' FROM LOAN GROUP BY EMPLOYEEID) AS T2 ON T1.id = T2.EMPLOYEEID " +
                            " LEFT JOIN( " +
                            " SELECT EMPLOYEEID, SUM(LOAN) AS 'Deduction' FROM Detection GROUP BY EMPLOYEEID) AS T3 ON T1.ID = T3.EMPLOYEEID " +
                            " ORDER BY T1.EMPLOYEEID";
                    break;
                case "EmployeeDetail":
                    query = "SELECT emp.employeeid,emp.name,emp.status,ISNULL(emp.insuranceCategory,'') AS biocode,ISNULL(emp.bisSalary,'0') AS bisSalary,dpt.name AS Department,ctg.name AS Category" +
                            " FROM EMPLOYEE emp " + " " +
                            " INNER JOIN Category ctg on ctg.id=emp.categoryId" + " " +
                            " inner join Department dpt on dpt.id = emp.departmentId" + " " +
                            " where emp.status ='Active' and emp.categoryId=@ID" +
                            " order by emp.employeeid";
                    break;
                case "DeductionReport":
                    query = "SELECT emp.employeeid,emp.name AS 'Name',FORMAT(dtn.date,'dd/MM/yyyy') AS 'Date',ISNULL(dtn.travelAllowance,0) AS 'Travel',ISNULL(dtn.loan,0) AS 'Loan',ISNULL(dtn.bonus,0) AS 'Bonus',ISNULL(dtn.advance,0) AS 'Advance',ISNULL(dtn.tds,0) AS 'TDS',ISNULL(dtn.cashVoucher,0) AS 'Cash',ISNULL(dtn.certificationFees,0) AS 'Fees'" + " " +
                        ",(ISNULL(dtn.travelAllowance,0)+ISNULL(dtn.loan,0)+ISNULL(dtn.bonus,0)+ISNULL(dtn.advance,0)+ISNULL(dtn.tds,0)+ISNULL(dtn.cashVoucher,0)+ISNULL(dtn.certificationFees,0)) As 'Total'" +
                        "FROM Detection dtn" + " " +
                        "INNER JOIN Employee emp on dtn.employeeId = emp.id" + " " +
                        "where FORMAT(dtn.date,'dd/MM/yyyy') BETWEEN @DATEFROM AND @DATETO " +
                        "group By dtn.employeeId,emp.employeeid,emp.id,emp.name,dtn.date,dtn.travelAllowance,dtn.loan,dtn.bonus,dtn.advance,dtn.tds,dtn.cashVoucher,dtn.certificationFees" + " " +
                        "Order by Date, employeeId; ";
                    break;
                case "TravelExpenseReport":
                    query = "SELECT emp.employeeid,emp.name,c.companyName,l.location,'-' AS 'stayDays',count(tc.employee_id) AS 'visitDays', " +
                            " ch.employeestaycharge as 'stayCharge',ch.employeevisitcharge as 'visitCharge'," +
                            " (count(tc.employee_id) * ch.employeevisitcharge) as 'totalVisitcharge','-' as totalStayCharge" +
                            " FROM EMPLOYEE emp" +
                            " INNER JOIN TPICALLS tc on tc.employee_id = emp.id" +
                            " INNER JOIN COMPANY c on c.id = tc.company_id" +
                            " INNER JOIN LOCATION l on l.id = c.locationid" +
                            " INNER JOIN CHARGEs ch on ch.locationid = l.id AND ch.companyid = c.id" +
                            " where emp.categoryid=@ID and FORMAT(tc.date,'dd/MM/yyyy') BETWEEN @DATEFROM AND @DATETO" +
                            " GROUP BY emp.employeeid,emp.name,c.companyName,l.location,ch.employeestaycharge,ch.employeevisitcharge";
                    break;
                case "EmployeeIDCard":
                    query = "SELECT emp.employeeId,emp.name,emp.address,FORMAT(emp.dob,'MM/dd/yyyy') As DOB,emp.bloodGroup,emp.phoneNumber,ISNULL(emp.photo,'No Photo') As photos" +
                            " FROM employee emp " +
                            " where emp.id=@ID";
                    break;
                case "AttendanceDetails":
                        query = "SELECT att.id AS ID,e.EMPLOYEEID,E.NAME, FORMAT(att.DATE,'MM-dd-yyyy') as DATE,att.STATUS,att.PFStatus,e.categoryId " +
                            " FROM ATTENDANCE att " +
                            " INNER JOIN EMPLOYEE e on e.ID = att.employeeid " +
                            " WHERE e.categoryID = @CATEGORYID and FORMAT(att.date,'yyyy-MM-dd')= @DATE";

                    break;
                case "monthAttendanceDetails":
                    query = " SELECT att.id AS ID,e.EMPLOYEEID,E.NAME, FORMAT(att.DATE,'MM/dd/yyyy') as DATE,att.present,att.abosent,att.PFAttendance" +
                            " FROM monthlyAttendances att" +
                            " left JOIN EMPLOYEE e on e.ID = att.employeeId" +
                            " WHERE FORMAT(DATE,'yyyy-MM')=@DATE   AND e.categoryID = @CATEGORYID";
                    break;
                case "SalaryProcess":
                    query = "SELECT T1.ID,T1.employeeid,T1.name,ISNULL(T2.Present, 0) As 'Present',ISNULL(T1.bissalary, 0) AS bissalary, ISNULL(T3.Travel, 0) As Travel, ISNULL(T3.Loan, 0) AS Loan, ISNULL(T3.Bonus, 0) AS Bonus, ISNULL(T3.Advance, 0) AS Advance, ISNULL(T3.Tds, 0) As Tds, " +
                            " ISNULL(T3.Cashvoucher, 0) As Cashvoucher, ISNULL(T3.CertificationFees, 0) AS CertificationFees,(ISNULL(t3.Advance, 0) + ISNULL(T3.Loan, 0) + ISNULL(T3.Tds, 0) + ISNULL(T3.CertificationFees, 0)) AS 'TotalDeduction', " +
                            " ISNULL(((T2.Present * T1.bissalary) + T3.Travel + T3.Bonus + T3.Cashvoucher), 0) As 'GrossSalary',ISNULL((T2.Present * T1.bissalary), 0) As 'ActualSalary', " +
                            " ISNULL((((T2.Present * T1.bissalary) + T3.Travel + T3.Bonus + T3.Cashvoucher) - (ISNULL(t3.Advance, 0) + ISNULL(T3.Loan, 0) + ISNULL(T3.Tds, 0) + ISNULL(T3.CertificationFees, 0))), 0) as NetSalary " +
                            " FROM " +
                            " (SELECT emp.ID, emp.employeeId, emp.name AS 'name', emp.bissalary " +
                            " FROM EMPLOYEE emp  " +
                            " WHERE emp.categoryId = @CATEGORY) as T1 " +
                            " LEFT JOIN " +
                            " (SELECT att.employeeid, count(att.employeeId) as 'Present' " +
                            " FROM ATTENDANCE att " +
                            " INNER JOIN EMPLOYEE emp on emp.id = att.employeeid " +
                            //" where emp.categoryId = @CATEGORY AND att.status = 'Present' AND att.date BETWEEN CONVERT(datetime, @DATEFROM, 103) AND CONVERT(datetime, @DATETO,103) " +
                            " where emp.categoryId = @CATEGORY AND att.status = 'Present' AND FORMAT( att.date,'yyyy-MM')=  @DATE" +
                            " group by att.employeeId) as T2 on T1.ID = T2.employeeid " +
                            " LEFT JOIN " +
                            " (SELECT d.employeeid, sum(d.travelallowance) as 'Travel',sum(d.loan) as 'Loan',sum(d.bonus) as 'Bonus',sum(d.advance) as 'Advance',  " +
                            " sum(d.tds) as 'TDS',sum(d.cashvoucher) as 'CashVoucher',sum(d.certificationfees) as 'CertificationFees' " +
                            " FROM DETECTION d " +
                            //" WHERE d.date BETWEEN CONVERT(datetime, @DATEFROM, 103) AND CONVERT(datetime, @DATETO,103)   " +
                            " WHERE FORMAT( d.date,'yyyy-MM')= @DATE   " +
                            " group by d.employeeid) AS T3 on T1.ID = T3.employeeid order by T1.employeeId";
                    //query = "SELECT T1.ID,T1.employeeid,T1.name,ISNULL(T2.Present,0) As 'Present',ISNULL(T1.bissalary,0) AS bissalary,ISNULL(T3.Travel,0) As Travel,ISNULL(T3.Loan,0) AS Loan,ISNULL(T3.Bonus,0) AS Bonus,ISNULL(T3.Advance,0) AS Advance,ISNULL(T3.Tds,0) As Tds, " +
                    //        " ISNULL(T3.Cashvoucher, 0) As Cashvoucher, ISNULL(T3.CertificationFees, 0) AS CertificationFees,(ISNULL(t3.Advance, 0) + ISNULL(T3.Loan, 0) + ISNULL(T3.Tds, 0) + ISNULL(T3.CertificationFees, 0)) AS 'TotalDeduction', " +
                    //        " ISNULL(((T2.Present * T1.bissalary) + T3.Travel + T3.Bonus + T3.Cashvoucher), 0) As 'GrossSalary',ISNULL((T2.Present * T1.bissalary), 0) As 'ActualSalary', " +
                    //        " ISNULL((((T2.Present * T1.bissalary) + T3.Travel + T3.Bonus + T3.Cashvoucher) - (ISNULL(t3.Advance, 0) + ISNULL(T3.Loan, 0) + ISNULL(T3.Tds, 0) + ISNULL(T3.CertificationFees, 0))), 0) as NetSalary " +
                    //        " FROM " +
                    //        " (SELECT emp.ID, emp.employeeId, emp.name AS 'name', emp.bissalary " +
                    //        " FROM EMPLOYEE emp  " +
                    //        " WHERE emp.categoryId = @CATEGORY) as T1 " +
                    //        " LEFT JOIN " +
                    //        " (SELECT att.employeeid, count(att.employeeId) as 'Present' " +
                    //        " FROM ATTENDANCE att " +
                    //        " INNER JOIN EMPLOYEE emp on emp.id = att.employeeid " +
                    //        " where emp.categoryId = @CATEGORY AND att.status = 'Present' AND att.date BETWEEN CONVERT(datetime, @DATEFROM, 103) AND CONVERT(datetime, @DATETO,103) " +
                    //        " group by att.employeeId) as T2 on T1.ID = T2.employeeid " +
                    //        " LEFT JOIN " +
                    //        " (SELECT d.employeeid, sum(d.travelallowance) as 'Travel',sum(d.loan) as 'Loan',sum(d.bonus) as 'Bonus',sum(d.advance) as 'Advance',  " +
                    //        " sum(d.tds) as 'TDS',sum(d.cashvoucher) as 'CashVoucher',sum(d.certificationfees) as 'CertificationFees' " +
                    //        " FROM DETECTION d " +
                    //        " WHERE d.date BETWEEN CONVERT(datetime, @DATEFROM, 103) AND CONVERT(datetime, @DATETO,103)   " +
                    //        " group by d.employeeid) AS T3 on T1.ID = T3.employeeid order by T1.employeeId";
                    break;
                case "DashBoardAttendance":
                    query = "SELECT ctg.ID,ctg.name As Category,ISNULL(T1.Present,0) AS 'Present',ISNULL(T2.Absent,0) AS 'Absent'," +
                            " ISNULL(T3.MonthPresent, 0) AS 'MonthPresent',ISNULL(T4.MonthAbsent, 0) AS 'MonthAbsent' " +
                            " FROM Category ctg" +
                            " LEFT JOIN" +
                            " (" +
                            " SELECT ctg.id, count(att.employeeId) as 'Present'" +
                            " FROM ATTENDANCE att" +
                            " INNER JOIN EMPLOYEE emp on emp.ID = att.employeeid" +
                            " INNER JOIN Category ctg on emp.categoryid = ctg.id" +
                            " WHERE FORMAT(att.date, 'MM/dd/yyyy') = @DATE AND att.STATUS = 'Present'" +
                            " GROUP BY ctg.id) AS T1 ON ctg.id = T1.id" +
                            " LEFT JOIN" +
                            " (" +
                            " SELECT ctg.id, ISNULL(count(att.employeeId),0) as 'Absent'" +
                            " FROM ATTENDANCE att" +
                            " INNER JOIN EMPLOYEE emp on emp.ID = att.employeeid" +
                            " INNER JOIN Category ctg on emp.categoryid = ctg.id" +
                            " WHERE FORMAT(att.date,'MM/dd/yyyy')= @DATE AND att.STATUS = 'Absent'" +
                            " GROUP BY ctg.id) AS T2 ON ctg.id = T2.id" +
                            " LEFT JOIN" +
                            " (" +
                            " SELECT ctg.id, count(att.employeeId) as 'MonthPresent'" +
                            " FROM ATTENDANCE att" +
                            " INNER JOIN EMPLOYEE emp on emp.ID = att.employeeid" +
                            " INNER JOIN Category ctg on emp.categoryid = ctg.id" +
                            " WHERE FORMAT(att.date,'MM/dd/yyyy') BETWEEN @FIRSTDATE AND @LASTDATE AND att.STATUS = 'Present'" +
                            " GROUP BY ctg.id) AS T3 ON ctg.id = T3.id" +
                            " LEFT JOIN" +
                            " (" +
                            " SELECT ctg.id, count(att.employeeId) as 'MonthAbsent'" +
                            " FROM ATTENDANCE att" +
                            " INNER JOIN EMPLOYEE emp on emp.ID = att.employeeid" +
                            " INNER JOIN Category ctg on emp.categoryid = ctg.id" +
                            " WHERE FORMAT(att.date,'MM/dd/yyyy') BETWEEN @FIRSTDATE AND @LASTDATE AND att.STATUS = 'Absent'" +
                            " GROUP BY ctg.id) AS T4 ON ctg.id = T4.id";
                    break;
                case "TodayAbsent":
                    query = "SELECT ISNULL(count(att.employeeId),0) as 'Absent' " +
                            " FROM ATTENDANCE att " +
                            " INNER JOIN EMPLOYEE emp on emp.ID = att.employeeid " +
                            " INNER JOIN Category ctg on emp.categoryid = ctg.id " +
                            " WHERE FORMAT(att.date,'MM/dd/yyyy')= (SELECT CONVERT(VARCHAR(10), getdate(), 101)) AND att.STATUS = 'Absent'; ";
                    break;
                case "TotalTPI":
                    query = "SELECT COUNT(ID) " +
                             "FROM TPICalls" + " " +
                             "WHERE FORMAT(date,'MM/dd/yyyy')= (SELECT CONVERT(VARCHAR(10), getdate(), 101))";
                    break;
                case "EmployeeDetailsByID":
                    query = "SELECT e.id,e.name,ISNULL(c.name,' ') as Emp_category " +
                            " FROM EMPLOYEE e " +
                            " LEFT JOIN CATEGORY c ON c.id = e.categoryid " +
                            " WHERE e.ID =@ID";
                    break;
                case "EmployeeDetailsByCategoryID":
                    query = "SELECT e.id,concat(e.employeeid,' - ',e.name) as name,ISNULL(c.name,' ') as Emp_category " +
                            " FROM EMPLOYEE e " +
                            " INNER JOIN CATEGORY c ON c.id = e.categoryid " +
                            " WHERE C.ID =@ID";
                    break;
                case "ExperienceCertificateReport":
                    query = "select emp.employeeId,emp.name,format(emp.doj,'dd/MM/yyyy') as doj,format(getdate(),'dd/MM/yyyy') as getdate " +
                             " from Employee emp Where emp.ID = @ID";
                    break;
                case "Employees":
                    query = "SELECT emp.Name,emp.EmployeeId,emp.doj,format(emp.dob,'dd/MM/yyyy') as dob,isnull(emp.email,'-') as email,isnull(emp.phoneNumber,'-') as phoneNumber," +
                        "emp.address," +
                        "emp.adharNumber,emp.panNo,emp.degree,emp.university,emp.percentage,emp.ndeQualificationType," +
                        "emp.expiryDate,emp.communicationAddress,emp.bloodGroup,emp.holderName,emp.periodFrom,emp.periodTo," +
                        "emp.salary,emp.uniformIssueDate,shoeIssueDate,emp.status,emp.institutionName,emp.yearofCompletion," +
                        "emp.salary,emp.bankName,emp.accountNo,emp.ifscCode,emp.esi,emp.institutionName,C.companyName FROM Employee emp INNER JOIN COMPANY C ON C.id = emp.companyId Where emp.ID=@ID";
                    break;
                case "MoneyTransferReport":
                    query = "Select emp.Id,emp.EmployeeId,emp.accountNo,emp.name,s.actualSalary,emp.salaryType,emp.ifscCode, format(s.date,'dd/MM/yyyy') as date " +
                            " from Employee emp " +
                            " left join salary s on s.employeeid = emp.id " +
                            //" where emp.id = @ID and FORMAT(s.date,'MM/yyyy') between @DATEFROM AND @DATEFROM";
                            " where emp.categoryId = @ID and (MONTH(s.date)= @MONTH) and (YEAR(s.date)=@YEAR) ";
                    break;
                case "MoneyTransferReportWithoutID":
                    query = "Select emp.Id,emp.EmployeeId,emp.accountNo,emp.name,s.actualSalary,emp.salaryType,emp.ifscCode, format(s.date,'dd/MM/yyyy') as date " +
                            " from Employee emp " +
                            " left join salary s on s.employeeid = emp.id " +
                            " where (MONTH(s.date)= @MONTH) and (YEAR(s.date)=@YEAR) ";
                    break;
                case "esipfreport":
                    //query = "select ROUND(ISNULL(convert(int,DA)+convert(int,Basic)+convert(int,HRA),0),2) as SALARYFORDAY,emp.employeeId,"
                    //        + "FORMAT((((ISNULL(mon.PFAttendance, 0) * ROUND(ISNULL(convert(int, DA) + convert(int, Basic) + convert(int, HRA), 0), 2)) * 3.25) / 100), 'N2') AS EMPLOYER_CONT, "
                    //       + "FORMAT((((ISNULL(mon.PFAttendance, 0) * ROUND(ISNULL(convert(int, DA) + convert(int, Basic) + convert(int, HRA), 0), 2)) * 0.75) / 100), 'N2') AS EMPLOYEE_CONT, "
                    //       + "FORMAT((((ISNULL(mon.PFAttendance, 0) * ISNULL(convert(int, DA) + convert(int, Basic) + convert(int, HRA), 0)) * 3.25) / 100) + "
                    //       + "(((ISNULL(mon.PFAttendance, 0) * ISNULL(convert(int, DA) + convert(int, Basic) + convert(int, HRA), 0)) * 0.75) / 100), 'N2') AS TOTAL_CONT, "
                    //       + " FORMAT((((ISNULL(mon.PFAttendance, 0) * ISNULL(convert(int, DA) + convert(int, Basic) + convert(int, HRA), 0))) - (((ISNULL(mon.PFAttendance, 0) * ROUND(ISNULL(convert(int, DA) + convert(int, Basic) + convert(int, HRA), 0), 2)) * 0.75) / 100)), 'N2') AS NET_SALARY, "
                    //       + " FORMAT((((ISNULL(mon.PFAttendance, 0) * ROUND(ISNULL(convert(int, DA) + convert(int, Basic) + convert(int, HRA), 0), 2)))), 'N2') AS GROSS_SALARY, "
                    //       + "ISNULL(mon.PFAttendance, 0) as WORKINGDAYS,mon.employeeName,cat.name AS 'Category' "
                    //       + "from Employee as emp left join monthlyAttendances mon on mon.employeeId = emp.id "
                    //       + "left join Category cat on emp.categoryId = cat.id where FORMAT(DATE,'yyyy-MM')= @DATE   AND emp.categoryID = @CATEGORYID";
                    query = "SELECT T1.ID,T1.EMPLOYEEID,T1.NAME,T1.CATEGORY,ISNULL(T2.Present,0) AS WORKINGDAYS,ISNULL(T1.salary,0) AS SALARY,"
                            + "ROUND((ISNULL(T2.Present, 0) * ISNULL(T1.salary, 0)), 2) AS GROSS_SALARY,"
                            + "FORMAT((((ISNULL(T2.Present, 0) * ISNULL(T1.salary, 0)) * 13) / 100), 'N2') AS EMPLOYER_CONT,"
                            + "FORMAT((((ISNULL(T2.Present, 0) * ISNULL(T1.salary, 0)) * 12) / 100), 'N2') AS EMPLOYEE_CONT,"
                            + "FORMAT(((((ISNULL(T2.Present, 0) * ISNULL(T1.salary, 0)) * 13) / 100) + (((ISNULL(T2.Present, 0) * ISNULL(T1.salary, 0)) * 12) / 100)), 'N2') AS TOTAL_CONT,"
                            + "FORMAT(((ISNULL(T2.Present, 0) * ISNULL(T1.salary, 0)) - (((ISNULL(T2.Present, 0) * ISNULL(T1.salary, 0)) * 12) / 100)), 'N2') AS NET_SALARY" + " "
                            + "FROM"
                            // + "(SELECT emp.id, emp.employeeId, emp.name, ctg.name AS 'Category',(Convert(int,emp.basic)+Convert(int,emp.DA)) as salary" + " "
                            + "(SELECT emp.id, emp.employeeId, emp.name, ctg.name AS 'Category',ISNULL((Convert(int,emp.basic)+Convert(int,emp.DA)),0) as salary" + " "
                            + "FROM EMPLOYEE emp  INNER JOIN CATEGORY ctg ON ctg.ID = emp.categoryId  WHERE ctg.ID = @CATEGORYID AND emp.insuranceCategory Like '%PF%') AS T1" + " "
                            + "LEFT JOIN"
                            + "(SELECT emp.employeeId, count(atd.employeeId) as 'Present'  FROM EMPLOYEE emp" + " "
                            + "INNER JOIN ATTENDANCE atd ON atd.employeeId = emp.id  WHERE atd.PFStatus = 'Present' AND  FORMAT(DATE,'yyyy-MM')= @DATE" + " "
                            + "GROUP BY emp.employeeId)"
                            + "AS T2 ON T1.employeeId = T2.employeeId";
                    break;
                case "esiReport":
                    query = "SELECT T1.ID,T1.EMPLOYEEID,T1.NAME,T1.CATEGORY,ISNULL(T2.Present,0) AS WORKINGDAYS,ISNULL(T1.salary,0) AS SALARY,"
               + "ROUND((ISNULL(T2.Present, 0) * ISNULL(T1.salary, 0)), 2) AS GROSS_SALARY,"
               + "FORMAT((((ISNULL(T2.Present, 0) * ISNULL(T1.salary, 0)) * 3.25) / 100), 'N2') AS EMPLOYER_CONT,"
               + "FORMAT((((ISNULL(T2.Present, 0) * ISNULL(T1.salary, 0)) * 0.75) / 100), 'N2') AS EMPLOYEE_CONT,"
               + "FORMAT(((((ISNULL(T2.Present, 0) * ISNULL(T1.salary, 0)) * 3.25) / 100) + (((ISNULL(T2.Present, 0) * ISNULL(T1.salary, 0)) * 0.75) / 100)), 'N2') AS TOTAL_CONT,"
               + "FORMAT(((ISNULL(T2.Present, 0) * ISNULL(T1.salary, 0)) - (((ISNULL(T2.Present, 0) * ISNULL(T1.salary, 0)) * 0.75) / 100)), 'N2') AS NET_SALARY" + " "
               + "FROM"
                + "(SELECT emp.id, emp.employeeId, emp.name, ctg.name AS 'Category',emp.salary as salary" + " "
               + "FROM EMPLOYEE emp  INNER JOIN CATEGORY ctg ON ctg.ID = emp.categoryId  WHERE ctg.ID = @CATEGORYID AND emp.insuranceCategory Like '%ESI%') AS T1" + " "
               + "LEFT JOIN"
               + "(SELECT emp.employeeId, count(atd.employeeId) as 'Present'  FROM EMPLOYEE emp" + " "
               + "INNER JOIN ATTENDANCE atd ON atd.employeeId = emp.id  WHERE atd.status = 'Present' AND  FORMAT(DATE,'yyyy-MM')= @DATE" + " "
               + "GROUP BY emp.employeeId)"
               + "AS T2 ON T1.employeeId = T2.employeeId"; ;
                    break;
                case "PaySlipReport":
                    query = "select emp.employeeId,emp.phoneNumber from Employee emp Where emp.ID=@ID";
                    break;
                case "VendorByCompanyID":
                    query = "SELECT ID,NAME FROM VENDOR WHERE COMPANYID=@ID";
                    break;
                case "VendorByLocationID":
                    query = "SELECT VENDOR.ID,Location.id AS LOCATIONID, LOCATION.LOCATION FROM VENDOR INNER JOIN LOCATION ON VENDOR.LOCATIONID = LOCATION.ID  WHERE VENDOR.ID=@ID";
                    break;
                case "DailyAttendanceReport":
                    query = "SELECT format(date,'dd/MM/yyyy') as date,c.name as category,e.employeeid,e.name,att.status,'' as time,"
                                + "ISNULL(e.biocode, '') as biocode,isnull(att.temperator, '') as temprature,isnull(att.mask, '') as mask" + " "
                                + "FROM ATTENDANCE att" + " "
                                + "INNER JOIN Employee e on e.id = att.employeeId" + " "
                                + "INNER JOIN Category c on c.id = e.categoryid" + " "
                                + "INNER JOIN Department d on d.id = e.departmentId" + " "
                                + "WHERE DATE BETWEEN @DATEFROM AND @DATETO AND e.categoryid = @ID" + " "
                                + "ORDER BY DATE,e.name;";
                    break;
                case "DailyAttendanceReportByEmp":
                    query = "SELECT format(date,'dd/MM/yyyy') as date,c.name as category,e.employeeid,e.name,att.status,'' as time,"
                                + "ISNULL(e.biocode, '') as biocode,isnull(att.temperator, '') as temprature,isnull(att.mask, '') as mask" + " "
                                + "FROM ATTENDANCE att" + " "
                                + "INNER JOIN Employee e on e.id = att.employeeId" + " "
                                + "INNER JOIN Category c on c.id = e.categoryid" + " "
                                + "WHERE DATE BETWEEN @DATEFROM AND @DATETO AND e.categoryid = @ID and e.id = @EMPLOYEEID" + " "
                                + "ORDER BY DATE,e.name;";
                    break;

                case "AnnexureReportByCategory":
                    query = "SELECT T1.employeeId,T1.Employee,T1.Category,T1.companyName as 'Company',T2.Days,ISNULL(T1.companyClaimCharges,0) as companyClaimCharges,"
                           + " ISNULL(T2.DAYS * T1.companyClaimCharges,0) as 'Charges' FROM"
                             + " (SELECT emp.employeeId, emp.name as 'Employee', c.name as 'Category', com.companyName, ch.companyClaimCharges"
                             + " FROM EMPLOYEE emp"
                             + " INNER JOIN CATEGORY c on c.id = emp.categoryid"
                             + " LEFT JOIN COMPANY com on com.id = emp.companyId"
                             + " LEFT JOIN CHARGES ch on ch.companyId = emp.companyId and ch.categoryId=emp.categoryId"
                             + " WHERE emp.categoryId = @ID"
                             + " ) AS T1"
                           + " INNER JOIN"
                           + " (SELECT emp.employeeId, count(atd.employeeId) as 'Days'"
                           + " FROM EMPLOYEE emp"
                           + " INNER JOIN ATTENDANCE atd ON atd.employeeId = emp.id"
                           + " WHERE atd.STATUS = 'Present' AND  FORMAT(DATE,'yyyy-MM') = @DATEFROM"
                           + " GROUP BY emp.employeeId ) AS T2 ON T1.employeeId = T2.employeeId"
                           + " ORDER BY T1.Category,T1.Employee; ";
                    break;
                case "AnnexureReportByCompany":
                    query = "SELECT T1.employeeId,T1.Employee,T1.Category,T1.companyName as 'Company',T2.Days,ISNULL(T1.companyClaimCharges,0) as companyClaimCharges,"
                            + " ISNULL(T2.DAYS * T1.companyClaimCharges,0) as 'Charges' FROM"
                             + " (SELECT emp.employeeId, emp.name as 'Employee', c.name as 'Category', com.companyName, ch.companyClaimCharges"
                             + " FROM EMPLOYEE emp"
                             + " INNER JOIN CATEGORY c on c.id = emp.categoryid"
                             + " LEFT JOIN COMPANY com on com.id = emp.companyId"
                             + " LEFT JOIN CHARGES ch on ch.companyId = emp.companyId and ch.categoryId=emp.categoryId"
                             + " WHERE emp.companyId = @COMPANYID"
                             + " ) AS T1"
                           + " INNER JOIN"
                            + " (SELECT emp.employeeId, count(atd.employeeId) as 'Days'"
                            + " FROM EMPLOYEE emp"
                            + " INNER JOIN ATTENDANCE atd ON atd.employeeId = emp.id"
                           + " WHERE atd.STATUS = 'Present' AND FORMAT(DATE,'yyyy-MM') = @DATEFROM"
                           //  + " WHERE atd.STATUS = 'Present' AND DATE BETWEEN @DATEFROM AND @DATETO"
                           + " GROUP BY emp.employeeId ) AS T2 ON T1.employeeId = T2.employeeId"
                           + " ORDER BY T1.Category,T1.Employee; ";
                    break;
                case "AnnexureReport":
                    query = "SELECT T1.employeeId,T1.Employee,T1.Category,T1.companyName as 'Company',T2.Days,ISNULL(T1.companyClaimCharges,0) as companyClaimCharges,"
                            + " ISNULL(T2.DAYS * T1.companyClaimCharges,0) as 'Charges' FROM"
                             + " (SELECT distinct emp.employeeId, emp.name as 'Employee', c.name as 'Category', com.companyName, ch.companyClaimCharges"
                             + " FROM EMPLOYEE emp"
                             + " INNER JOIN CATEGORY c on c.id = emp.categoryid"
                             + " LEFT JOIN COMPANY com on com.id = emp.companyId"
                             + " LEFT JOIN CHARGES ch on ch.companyId = emp.companyId and ch.categoryId=emp.categoryId"
                             + " WHERE emp.companyId = @COMPANYID and  emp.categoryId = @ID"
                             + " ) AS T1"
                           + " INNER JOIN"
                            + " (SELECT emp.employeeId, count(atd.employeeId) as 'Days'"
                            + " FROM EMPLOYEE emp"
                            + " INNER JOIN ATTENDANCE atd ON atd.employeeId = emp.id"
                           + " WHERE atd.STATUS = 'Present' AND FORMAT(DATE,'yyyy-MM') = @DATEFROM"
                           //  + " WHERE atd.STATUS = 'Present' AND DATE BETWEEN @DATEFROM AND @DATETO"
                           + " GROUP BY emp.employeeId ) AS T2 ON T1.employeeId = T2.employeeId"
                           + " ORDER BY T1.Category,T1.Employee; ";
                    break;

                case "TPIAnnexure":
                    query = "SELECT T1.employeeid,T1.name,T1.location,T1.companyname,T2.stay," +
                            " isnull(T2.Visit,0) as Visit FROM (SELECT distinct tpa.id, e.employeeid, e.name, e.companyId, l.location, c.companyName FROM EMPLOYEE e " +
                            " INNER JOIN TPIAllocations tpa on tpa.employeeid = e.id " +
                            " INNER JOIN COMPANY c on c.id = tpa.companyid " +
                            " INNER JOIN Location l on l.id = tpa.locationid WHERE tpa.companyId = @ID ) AS T1 " +
                            " LEFT JOIN( SELECT distinct tpa.id, tpa.employeeid, tpa.companyid," +
                            " COUNT(tpa.id) as 'Visit',tpc.stay as 'stay' FROM TPIAllocations tpa " +
                            " INNER JOIN TPICALLS tpc on tpa.id = tpc.tPIAllocationId " +
                            " WHERE  FORMAT(tpc.inTime,'yyyy-MM') = @DATEFROM " +
                            " group by tpa.id,tpc.tpiallocationid,tpa.employeeid,tpa.companyid,tpc.stay) AS T2 on T1.id = t2.id " +
                            " where Visit> 0 " +
                            " GROUP BY T1.employeeid,T1.name,T1.companyname,T1.location,T2.Visit,T2.stay order by T1.employeeid,T1.location,T1.companyname; ";
                    break;

                case "TPICallAnnexure":
                    query = "SELECT tpc.inTime as 'Date',e.name,l.location,v.name as 'CallRaiseBy',tpc.plant,tpc.productGroup, "
                            + "format(tpc.intime, 'hh:mm tt') as inTime,tpc.offeringTime,format(tpc.outTime, 'hh:mm tt') as outTime,tpc.idleTime, "
                            + "tpc.days,tpc.totalQTYoffered,tpc.noofOkCasting,tpc.ftp,tpc.stp,tpc.rw,tpc.hold,tpc.rejected,tpc.scopeinspection "
                            + "FROM TPICALLS tpc "
                            + "INNER JOIN TPIAllocations tpa on tpa.id = tpc.tPIAllocationId "
                            + "INNER JOIN employee e on e.id = tpa.employeeid "
                            + "INNER JOIN location l on l.id = tpa.locationId "
                            + "INNER JOIN vendor v on v.id = tpa.vendorid "
                            + "WHERE  FORMAT(tpc.inTime,'yyyy-MM') = @DATEFROM  AND tpa.companyId = @ID "
                            // + "WHERE tpc.inTime between @DATEFROM AND @DATETO AND tpa.companyId = @ID "
                            + "ORDER BY tpc.tPIAllocationId; ";
                    break;
                case "monthlyAttendanceReport":
                    query = "SELECT format(date,'MM/yyyy') as date,c.name as category,e.employeeid,e.name,att.present, "
                             + " isnull(att.abosent, '0') as Absent "
                             + " FROM monthlyAttendances att "
                             + " INNER JOIN Employee e on e.id = att.employeeId "
                             + " INNER JOIN Category c on c.id = e.categoryid "
                             + "WHERE FORMAT(DATE,'yyyy-MM') = @DATE AND e.categoryID = @ID "
                             + " ORDER BY DATE,e.name; ";
                    break;
                case "monthlyAttendanceReportBYempID":
                    query = "SELECT format(date,'MM/yyyy') as date,c.name as category,e.employeeid,e.name,att.present, "
                             + "isnull(att.abosent, '0') as Absent"
                             + "  FROM monthlyAttendances att "
                             + " INNER JOIN Employee e on e.id = att.employeeId "
                             + " INNER JOIN Category c on c.id = e.categoryid "
                             + "WHERE FORMAT(DATE,'yyyy-MM') = @DATE AND e.categoryID = @ID and att.employeeId = @EMPID"
                             + " ORDER BY DATE,e.name; ";
                    break;
                case "monthlysalaryProcess":
                    query = " SELECT T1.ID,T1.employeeid,T1.name,ISNULL(T2.Present, 0) As 'Present',ISNULL(T1.bissalary, 0) AS bissalary, ISNULL(T3.Travel, 0) As Travel, ISNULL(T3.Loan, 0) AS Loan, ISNULL(T3.Bonus, 0) AS Bonus, ISNULL(T3.Advance, 0) AS Advance, ISNULL(T3.Tds, 0) As Tds, " +
                    " ISNULL(T3.Cashvoucher, 0) As Cashvoucher, ISNULL(T3.CertificationFees, 0) AS CertificationFees,(ISNULL(t3.Advance, 0) + ISNULL(T3.Loan, 0) + ISNULL(T3.Tds, 0) + ISNULL(T3.CertificationFees, 0)) AS 'TotalDeduction',  " +
                     "  ISNULL(((T2.Present * T1.bissalary) + T3.Travel + T3.Bonus + T3.Cashvoucher), 0) As 'GrossSalary',ISNULL((T2.Present * T1.bissalary), 0) As 'ActualSalary',  " +
                     "  ISNULL((((T2.Present * T1.bissalary) + T3.Travel + T3.Bonus + T3.Cashvoucher) - (ISNULL(t3.Advance, 0) + ISNULL(T3.Loan, 0) + ISNULL(T3.Tds, 0) + ISNULL(T3.CertificationFees, 0))), 0) as NetSalary " +
                      " FROM " +
                     "  (SELECT emp.ID, emp.employeeId, emp.name AS 'name', emp.bissalary " +
                     "  FROM EMPLOYEE emp " +
                     "  WHERE emp.categoryId = @CATEGORY) as T1 " +
                     "  LEFT JOIN " +
                     "  (SELECT att.employeeid, sum(att.employeeId) as 'Present' " +
                     " FROM Attendance att  " +
                      //" FROM monthlyAttendances att " +
                      " INNER JOIN EMPLOYEE emp on emp.id = att.employeeid " +
                      " where emp.categoryId = @CATEGORY AND (MONTH(att.date)= @MONTH) and (YEAR(att.date)=@YEAR) and att.status='Present'" +
                      " group by att.employeeId) as T2 on T1.ID = T2.employeeid " +
                      " LEFT JOIN " +
                      " (SELECT d.employeeid, sum(d.travelallowance) as 'Travel',sum(d.loan) as 'Loan',sum(d.bonus) as 'Bonus',sum(d.advance) as 'Advance',   " +
                      " sum(d.tds) as 'TDS',sum(d.cashvoucher) as 'CashVoucher',sum(d.certificationfees) as 'CertificationFees' " +
                      " FROM DETECTION d " +
                    "  WHERE (MONTH(d.date)= @MONTH) and (YEAR(d.date)=@YEAR)  " +
                     "  group by d.employeeid) AS T3 on T1.ID = T3.employeeid order by T3.employeeId";
                    break;
                case "EmployeeDetailFORALL":
                    query = "SELECT emp.employeeid,emp.name,emp.status,ISNULL(emp.insuranceCategory,'') AS biocode,dpt.name AS Department,ctg.name AS Category,ISNULL(emp.salary,0) as bisSalary " +
                            " FROM EMPLOYEE emp " + " " +
                            " INNER JOIN Category ctg on ctg.id=emp.categoryId" + " " +
                            " inner join Department dpt on dpt.id = emp.departmentId" + " " +
                            " where emp.status ='Active'" +
                            " order by emp.employeeid";
                    break;
                case "employeeID":
                    query = "select top(1) employeeId+1 from Employee order by id desc";
                    break;
                case "EmployeeDetailsByDepartmentID":
                    query = "SELECT e.id,concat(e.employeeid,' - ',e.name) as name,ISNULL(d.name,' ') as Emp_category " +
                             " FROM EMPLOYEE e" +
                            " INNER JOIN Department d ON d.id = e.categoryid" +
                            " WHERE d.ID =@ID";
                    break;
                case "TPIReport":
                    query = "SELECT T1.id,T1.employeeid,T2.date,T1.name,T1.location,T1.companyname,T1.Vendor," +
                            " isnull(T2.Visit,0) as Visit," +
                            " ISNULL(T2.stay,0) AS stay," +
                            " ISNULL(SUM(T2.companyVisitCharge  * T2.visit),0) as VisitCharge," +
                            " ISNULL(T2.companyStayCharge,0) as StayCharge" +
                            " FROM (SELECT distinct tpa.id, tpa.employeeid, e.name, tpa.companyid, l.location, c.companyName,v.name as Vendor FROM EMPLOYEE e" +
                            " INNER JOIN TPIAllocations tpa on tpa.employeeid = e.id" +
                            " INNER JOIN COMPANY c on c.id = tpa.companyid" +
                            " INNER JOIN Location l on l.id = tpa.locationid" +
                            " INNER JOIN Vendor v on v.id = tpa.vendorId WHERE tpa.companyid = @ID) AS T1" +
                            " LEFT JOIN( SELECT distinct tpa.id, tpa.employeeid,tpc.date, tpa.companyId,ch.companyVisitCharge,ch.companyStayCharge, COUNT(tpa.id) as 'Visit'," +
                            " tpc.stay as 'stay' FROM TPIAllocations tpa" +
                            " INNER JOIN TPICALLS tpc on tpa.id = tpc.tPIAllocationId" +
                            " INNER JOIN Charges ch on ch.companyId=tpa.companyId WHERE  tpc.date" +
                            " BETWEEN @DATEFROM AND @DATETO" +
                            " group by tpa.id,tpc.tpiallocationid,tpa.employeeid,tpc.date,tpa.companyid,ch.companyVisitCharge,ch.companyStayCharge,tpc.stay) AS T2 on T1.id = t2.id" +
                            " where Visit> 0" +
                            " GROUP BY T1.id,T1.employeeid,T1.name,T1.companyname,T1.location," +
                            " T2.Visit,T2.stay,T1.Vendor,T2.companyVisitCharge,T2.companyStayCharge,T2.date order by T1.employeeid,T1.location,T1.companyname;";

                    break;
                case "TPIReports":
                    query = "SELECT T1.id,T1.employeeid,T2.date,T1.name,T1.location,T1.companyname,T1.Vendor," +
                            " isnull(T2.Visit,0) as Visit," +
                            " ISNULL(T2.stay,0) AS stay," +
                            " ISNULL(SUM(T2.employeeVisitCharge  * T2.visit),0) as VisitCharge," +
                            " ISNULL(T2.employeeStayCharge,0) as StayCharge" +
                            " FROM (SELECT distinct tpa.id, tpa.employeeid, e.name, tpa.companyid, l.location, c.companyName,v.name as Vendor FROM EMPLOYEE e" +
                            " INNER JOIN TPIAllocations tpa on tpa.employeeid = e.id" +
                            " INNER JOIN COMPANY c on c.id = tpa.companyid" +
                            " INNER JOIN Location l on l.id = tpa.locationid" +
                            " INNER JOIN Vendor v on v.id = tpa.vendorId WHERE tpa.employeeId = @EmployeeID) AS T1" +
                            " LEFT JOIN( SELECT distinct tpa.id, tpa.employeeid,tpc.date, tpa.companyId,ch.employeeVisitCharge,ch.employeeStayCharge, COUNT(tpa.id) as 'Visit'," +
                            " tpc.stay as 'stay' FROM TPIAllocations tpa" +
                            " INNER JOIN TPICALLS tpc on tpa.id = tpc.tPIAllocationId" +
                            " INNER JOIN Charges ch on ch.companyId=tpa.companyId WHERE  tpc.date" +
                            " BETWEEN @DATEFROM AND @DATETO" +
                            " group by tpa.id,tpc.tpiallocationid,tpa.employeeid,tpc.date,tpa.companyid,ch.employeeVisitCharge,ch.employeeStayCharge,tpc.stay) AS T2 on T1.id = t2.id" +
                            " where Visit> 0" +
                            " GROUP BY T1.id,T1.employeeid,T1.name,T1.companyname,T1.location," +
                            " T2.Visit,T2.stay,T1.Vendor,T2.employeeVisitCharge,T2.employeeStayCharge,T2.date order by T1.employeeid,T1.location,T1.companyname;";

                    break;
                case "ShiftReportAll":
                    query = "SELECT format(date,'dd/MM/yyyy') as date,c.name as category,e.employeeid,e.name,s.name as shifts,co.companyName "   
                                + "FROM ATTENDANCE att" + " "
                                + "INNER JOIN Employee e on e.id = att.employeeId" + " "
                                + " INNER JOIN shifts s on s.id = e.shiftId" + " "
                                + "INNER JOIN Company co on co.id = e.companyId" + " "
                                + "INNER JOIN Category c on c.id = e.categoryid " + " "
                                + "WHERE DATE BETWEEN @DATEFROM AND @DATETO AND e.categoryid = @ID  AND e.shiftId = @SHIFT" + " "
                                + "ORDER BY DATE,e.name;";
                    break;

            }
            return query;
        }
    }
}