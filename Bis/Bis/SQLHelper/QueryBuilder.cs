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
                    query = "SELECT T1.ID,T1.EMPLOYEEID,T1.NAME,T1.CATEGORY,ISNULL(T2.Present,0) AS PRESENT,ISNULL(T3.Absent,0) AS ABSENT " +
                                " FROM " +
                                " (SELECT emp.id,emp.employeeId,emp.name,ctg.name AS 'Category' " +
                                " FROM EMPLOYEE emp " +
                                " INNER JOIN CATEGORY ctg ON ctg.ID=emp.categoryId " +
                                " WHERE ctg.ID=@ID " +
                                " ) AS T1 " +
                                " LEFT JOIN " +
                                " (SELECT emp.employeeId,count(atd.employeeId) as 'Present' " +
                                " FROM EMPLOYEE emp " +
                                " INNER JOIN ATTENDANCE atd ON atd.employeeId=emp.id " +
                                " WHERE atd.STATUS='Present' AND DATE BETWEEN @DATEFROM AND @DATETO " +
                                " GROUP BY emp.employeeId) AS T2 ON T1.employeeId=T2.employeeId " +
                                " LEFT JOIN " +
                                " (SELECT emp.employeeId,count(atd.employeeId) as 'Absent' " +
                                " FROM EMPLOYEE emp " +
                                " INNER JOIN ATTENDANCE atd ON atd.employeeId=emp.id " +
                                " WHERE atd.STATUS='Absent' AND DATE BETWEEN @DATEFROM AND @DATETO " +
                                " GROUP BY emp.employeeId) AS T3 ON T1.employeeId=T3.employeeId";
                    break;
                case "AdvanceReport":
                    query = "SELECT emp.employeeid,emp.name AS 'Name',ctg.name AS 'Category',FORMAT(adv.date,'dd-MM-yyyy') AS 'Date',SUM(adv.amount) AS 'Advance'" +
                        "FROM Advance adv" + " " +
                        "INNER JOIN Employee emp on adv.employeeId = emp.id" + " " +
                        "inner join Category ctg on ctg.id = emp.categoryId" + " " +
                        "where ctg.id = @ID" + " " +
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
                    query = "SELECT emp.employeeid,emp.name,emp.status,ISNULL(emp.biocode,'') AS biocode,dpt.name AS Department,ctg.name AS Category" +
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
                    query = "SELECT att.id AS ID,e.EMPLOYEEID,E.NAME, FORMAT(att.DATE,'MM/dd/yyyy') as DATE,att.STATUS " +
                            " FROM ATTENDANCE att " +
                            " INNER JOIN EMPLOYEE e on e.ID = att.employeeid " +
                            " WHERE FORMAT(DATE,'dd/MM/yyyy')= @DATE AND e.categoryID=@CATEGORYID";
                    break;
                case "SalaryProcess":
                    query = "SELECT T1.ID,T1.employeeid,T1.name,ISNULL(T2.Present,0) As 'Present',ISNULL(T1.bissalary,0) AS bissalary,ISNULL(T3.Travel,0) As Travel,ISNULL(T3.Loan,0) AS Loan,ISNULL(T3.Bonus,0) AS Bonus,ISNULL(T3.Advance,0) AS Advance,ISNULL(T3.Tds,0) As Tds, " +
                            " ISNULL(T3.Cashvoucher,0) As Cashvoucher,ISNULL(T3.CertificationFees,0) AS CertificationFees,(ISNULL(t3.Advance, 0) + ISNULL(T3.Loan, 0) + ISNULL(T3.Tds, 0) + ISNULL(T3.CertificationFees, 0)) AS 'TotalDeduction', " +
                            " ((T2.Present * T1.bissalary) + T3.Travel + T3.Bonus + T3.Cashvoucher) As 'GrossSalary',(T2.Present * T1.bissalary) As 'ActualSalary'," +
                            " (((T2.Present * T1.bissalary) + T3.Travel + T3.Bonus + T3.Cashvoucher) - (ISNULL(t3.Advance, 0) + ISNULL(T3.Loan, 0) + ISNULL(T3.Tds, 0) + ISNULL(T3.CertificationFees, 0))) as NetSalary" +
                            " FROM " +
                            " (SELECT emp.ID, emp.employeeId, emp.name AS 'name', emp.bissalary " +
                            " FROM EMPLOYEE emp " +
                            " WHERE emp.categoryId = @CATEGORY) as T1 " +
                            " LEFT JOIN " +
                            " (SELECT att.employeeid, count(att.employeeId) as 'Present'" +
                            " FROM ATTENDANCE att " +
                            " INNER JOIN EMPLOYEE emp on emp.id = att.employeeid " +
                            " where emp.categoryId = @CATEGORY AND att.status = 'Present' AND FORMAT(att.date,'dd/MM/yyyy') BETWEEN @DATEFROM AND @DATETO " +
                            " group by att.employeeId) as T2 on T1.ID = T2.employeeid " +
                            " LEFT JOIN " +
                            " (SELECT d.employeeid, sum(d.travelallowance) as 'Travel',sum(d.loan) as 'Loan',sum(d.bonus) as 'Bonus',sum(d.advance) as 'Advance', " +
                            " sum(d.tds) as 'TDS',sum(d.cashvoucher) as 'CashVoucher',sum(d.certificationfees) as 'CertificationFees' " +
                            " FROM DETECTION d " +
                            " WHERE FORMAT(d.date, 'dd/MM/yyyy') between @DATEFROM and @DATETO " +
                            " group by d.employeeid) AS T3 on T1.ID = T3.employeeid";
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
                        "emp.salary,emp.bankName,emp.accountNo,emp.ifscCode,emp.esi,emp.institutionName FROM Employee emp Where emp.ID=@ID";
                    break;
                case "MoneyTransferReport":
                    query = "Select emp.EmployeeId,emp.accountNo,emp.name,s.actualSalary,emp.salaryType,emp.ifscCode, format(s.date,'dd/MM/yyyy') as date " +
                            " from Employee emp "+
                            " left join salary s on s.employeeid = emp.id "+
                            " where emp.id = @ID and FORMAT(s.date,'dd/MM/yyyy') between @DATEFROM AND @DATEFROM";
                    break;
                case "esipfreport":
                    query = "SELECT T1.ID,T1.EMPLOYEEID,T1.NAME,T1.CATEGORY,ISNULL(T2.Present,0) AS WORKINGDAYS,ISNULL(T1.salary,0) AS SALARY,"
                            + "ROUND((ISNULL(T2.Present, 0) * ISNULL(T1.salary, 0)), 2) AS GROSS_SALARY,"
                            + "FORMAT((((ISNULL(T2.Present, 0) * ISNULL(T1.salary, 0)) * 3.25) / 100), 'N2') AS EMPLOYER_CONT,"
                            + "FORMAT((((ISNULL(T2.Present, 0) * ISNULL(T1.salary, 0)) * 0.75) / 100), 'N2') AS EMPLOYEE_CONT,"
                            + "FORMAT(((((ISNULL(T2.Present, 0) * ISNULL(T1.salary, 0)) * 3.25) / 100) + (((ISNULL(T2.Present, 0) * ISNULL(T1.salary, 0)) * 0.75) / 100)), 'N2') AS TOTAL_CONT,"
                            + "FORMAT(((ISNULL(T2.Present, 0) * ISNULL(T1.salary, 0)) - (((ISNULL(T2.Present, 0) * ISNULL(T1.salary, 0)) * 0.75) / 100)), 'N2') AS NET_SALARY"+" "
                            + "FROM"
                            + "(SELECT emp.id, emp.employeeId, emp.name, ctg.name AS 'Category', emp.salary as salary"+" "
                            + "FROM EMPLOYEE emp  INNER JOIN CATEGORY ctg ON ctg.ID = emp.categoryId  WHERE ctg.ID = @ID) AS T1"+" "
                            + "LEFT JOIN"
                            + "(SELECT emp.employeeId, count(atd.employeeId) as 'Present'  FROM EMPLOYEE emp"+" "
                            + "INNER JOIN ATTENDANCE atd ON atd.employeeId = emp.id  WHERE atd.STATUS = 'Present' AND DATE BETWEEN @DATEFROM AND @DATETO"+" "
                            +"GROUP BY emp.employeeId)"
                            + "AS T2 ON T1.employeeId = T2.employeeId";
                    break;
                case "PaySlipReport":
                    query = "select emp.employeeId,emp.phoneNumber from Employee emp Where emp.ID=@ID";
                    break;
                case "VendorByCompanyID":
                    query = "SELECT ID,NAME FROM VENDOR WHERE COMPANYID=@ID";
                    break;
                case "VendorByLocationID":
                    query = "SELECT VENDOR.ID, LOCATION.LOCATION FROM VENDOR INNER JOIN LOCATION ON VENDOR.LOCATIONID = LOCATION.ID  WHERE VENDOR.ID=@ID";
                    break;
                case "DailyAttendanceReport":
                    query = "SELECT c.name as category,e.employeeid,e.name,ISNULL(e.biocode,'') as temperature,isnull(att.temperator,'') as biocode,isnull(att.mask,'') as mask"+" "+
                                "FROM ATTENDANCE att"+" "+
                                "INNER JOIN Employee e on e.id = att.employeeId"+" "+
                                "INNER JOIN Category c on c.id = e.categoryid"+" "+
                                "WHERE DATE BETWEEN @DATEFROM AND @DATETO AND e.categoryid = @ID";
                    break;
                case "DailyAttendanceReportByEmp":
                    query = "SELECT c.name as category,e.employeeid,e.name,ISNULL(e.biocode,'') as temperature,isnull(att.temperator,'') as biocode,isnull(att.mask,'') as mask" + " " +
                               "FROM ATTENDANCE att" + " " +
                               "INNER JOIN Employee e on e.id = att.employeeId" + " " +
                               "INNER JOIN Category c on c.id = e.categoryid" + " " +
                               "WHERE DATE BETWEEN @DATEFROM AND @DATETO AND e.categoryid = @ID AND e.id=@EMPLOYEEID";
                    break;
            }
            return query;
        }
    }
}