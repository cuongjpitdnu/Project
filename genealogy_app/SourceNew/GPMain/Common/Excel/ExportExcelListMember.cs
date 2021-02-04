using GPConst;
using GPMain.Common.Database;
using GPMain.Common.Helper;
using GPMain.Core;
using GPModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GPMain.Common.Excel
{
    public class ExportExcelListMember : ExcelCommon
    {
        private DataTable tableMember;
        public ExportExcelListMember()
        {
            tableMember = new DataTable();
            string[] columns = Enum.GetNames(typeof(ListMemberExcelEntity.Columns));
            foreach (string col in columns)
            {
                string nameColumn = ListMemberExcelEntity.NameColumns[col];
                nameColumn = string.IsNullOrEmpty(nameColumn) ? col : nameColumn;
                tableMember.Columns.Add(nameColumn);
            }
        }
        public void Export(List<ExTMember> lstMember, string tempPath, string pathSave, ProgressBarManager progressBar)
        {
            if (lstMember == null)
            {
                lstMember = new List<ExTMember>();
            }

            using (MemberHelper memberHelper = new MemberHelper())
            {
                int StartColumn = 1;
                int LastColumn = Enum.GetNames(typeof(ListMemberExcelEntity.Columns)).Length;
                Stopwatch sw = new Stopwatch();

                sw.Restart();

                //get all member
                var dicAllMember = memberHelper.FindExTMemberOutDictionary("", "", "");

                if (progressBar != null)
                {
                    progressBar.total = lstMember.Count * 2 + 1;
                    progressBar.count = 0;
                }

                //get all relation
                var dicRelationTMember = AppManager.DBManager.GetTable<TMemberRelation>().AsEnumerable().ToDictionary(x => x.memberId + x.relMemberId);
                int numMember = 1;

                List<int> lstIndex = new List<int>();
                List<List<int>> lstIndxSpouse = new List<List<int>>();

                lstMember.ForEach(mem =>
                {
                    List<int> lstNumberChildOfSpouse = new List<int>();
                    List<string> lstChildTemp = new List<string>();

                    int idx = 0;

                    //Thông tin cơ bản của thành viên
                    DataRow infoMember = tableMember.NewRow();
                    infoMember[(int)ListMemberExcelEntity.Columns.STT] = numMember;
                    infoMember[(int)ListMemberExcelEntity.Columns.LevelInFamily] = mem.LevelInFamilyForShow;
                    infoMember[(int)ListMemberExcelEntity.Columns.MemberName] = mem.Name;
                    infoMember[(int)ListMemberExcelEntity.Columns.Gender] = mem.GenderShow;
                    infoMember[(int)ListMemberExcelEntity.Columns.BirthDay] = mem.BirthdayShow;
                    infoMember[(int)ListMemberExcelEntity.Columns.DeadDay] = mem.IsDeath ? mem.DeadDayLunarShow : "";
                    //Lấy danh sách cha, mẹ, vợ/chồng của thành viên
                    var dicMemberRelation = GetMemberRelation2(dicAllMember, dicRelationTMember, mem);  // Co the toi uu

                    ExTMember father = GetValues<ExTMember>(dicMemberRelation, "Father");
                    ExTMember mother = GetValues<ExTMember>(dicMemberRelation, "Mother");
                    List<ExTMember> lstspouseMember = GetValues<List<ExTMember>>(dicMemberRelation, "Spouse");

                    infoMember[(int)ListMemberExcelEntity.Columns.Father] = father.Name;
                    infoMember[(int)ListMemberExcelEntity.Columns.Mother] = mother.Name;

                    if (lstspouseMember.Count == 0)
                    {
                        if (mem.ListCHILDREN == null || mem.ListCHILDREN.Count == 0)
                        {
                            tableMember.Rows.Add(infoMember);
                            idx += 1;
                        }
                        else
                        {
                            var numberChild = GetListInfoChildrenOfMember(dicAllMember, dicRelationTMember, mem, infoMember);
                            idx += numberChild;
                            lstNumberChildOfSpouse.Add(numberChild - 1);
                        }
                    }
                    else
                    {
                        //get list child of spouse
                        GetListInfoOfSpouse(dicAllMember, dicRelationTMember, mem, lstspouseMember[0], infoMember, ref lstNumberChildOfSpouse, ref idx, ref lstChildTemp);

                        //Thông tin các bà vợ còn lại(nếu có)
                        foreach (var spouseMember in lstspouseMember)
                        {
                            if (spouseMember == lstspouseMember[0]) continue;
                            DataRow newInfoMember = tableMember.NewRow();
                            GetListInfoOfSpouse(dicAllMember, dicRelationTMember, mem, spouseMember, newInfoMember, ref lstNumberChildOfSpouse, ref idx, ref lstChildTemp);
                        };

                        var lstChildMember = mem.ListCHILDREN
                                                .Where(x => !lstChildTemp.Contains(x) && dicAllMember.ContainsKey(x) && dicAllMember[x] != null)
                                                .Select(x => dicAllMember[x].Name)
                                                .ToList();

                        foreach (string child in lstChildMember)
                        {
                            DataRow info = tableMember.NewRow();
                            info[(int)ListMemberExcelEntity.Columns.Child] = child;
                            tableMember.Rows.Add(info);
                        }

                    }

                    if (progressBar != null)
                    {
                        progressBar.Percent = progressBar.fncCalculatePercent(++progressBar.count, progressBar.total);
                    }

                    lstIndex.Add(idx);
                    lstIndxSpouse.Add(lstNumberChildOfSpouse);
                    numMember++;
                });

                //Khởi tạo workbook mới

                CreateWorkbook(tempPath, pathSave);

                _objWorkbook.SuspendEvents();

                //Chèn thông tin ngày xuất bản
                SetValue((int)ListMemberExcelEntity.Rows.DateCreate, (int)ListMemberExcelEntity.Columns.STT + 1, $"Ngày xuất bản: {DateTime.Now.ToString("HH:mm dd/MM/yyyy")}");

                //Chèn thông tin danh sách thành viên
                InsertDataTable((int)ListMemberExcelEntity.Rows.DataStart - 1, (int)ListMemberExcelEntity.Columns.STT + 1, tableMember);

                //Cài đặt đường viền
                SetBorderRangeCell((int)ListMemberExcelEntity.Rows.DataStart, (int)ListMemberExcelEntity.Rows.DataStart + tableMember.Rows.Count - 1, StartColumn, LastColumn);
              
                //Cài đặt tự động thay đổi độ rộng của cột
                SetAutoWidthRangeColumn(StartColumn, LastColumn);

                //Gộp các ô dữ liệu
                MergeRowMemberAndSpouse(lstMember, lstIndex, lstIndxSpouse, progressBar);

                _objWorkbook.ResumeEvents();
                //Lưu file
                Save();
                File.Delete(tempPath);
                sw.Stop();
            }
            if (progressBar != null)
            {
                progressBar.Percent = progressBar.fncCalculatePercent2(++progressBar.count, progressBar.total);
            }
        }

        private void MergeRowMemberAndSpouse(List<ExTMember> lstMember, List<int> lstIndex, List<List<int>> lstIndxSpouse, ProgressBarManager progressBar)
        {
            int rowData = (int)ListMemberExcelEntity.Rows.DataStart;
            int cnt = 0;

            lstMember.ForEach(data =>
            {
                int rowEnd = rowData + lstIndex[cnt] - 1;

                if (rowEnd > rowData)
                {
                    MergeCells((int)ListMemberExcelEntity.Columns.STT + 1, rowData, rowEnd);
                    MergeCells((int)ListMemberExcelEntity.Columns.LevelInFamily + 1, rowData, rowEnd);
                    MergeCells((int)ListMemberExcelEntity.Columns.MemberName + 1, rowData, rowEnd);
                    MergeCells((int)ListMemberExcelEntity.Columns.Gender + 1, rowData, rowEnd);
                    MergeCells((int)ListMemberExcelEntity.Columns.BirthDay + 1, rowData, rowEnd);
                    MergeCells((int)ListMemberExcelEntity.Columns.DeadDay + 1, rowData, rowEnd);
                    MergeCells((int)ListMemberExcelEntity.Columns.Father + 1, rowData, rowEnd);
                    MergeCells((int)ListMemberExcelEntity.Columns.Mother + 1, rowData, rowEnd);
                    //_objWorksheet.Range(rowData, (int)ListMemberExcelEntity.Columns.STT + 1, rowEnd, (int)ListMemberExcelEntity.Columns.STT + 1)
                    //             .CopyTo(_objWorksheet.Range(rowData, (int)ListMemberExcelEntity.Columns.LevelInFamily + 1, rowEnd, (int)ListMemberExcelEntity.Columns.LevelInFamily + 1));
                    //_objWorksheet.Range(rowData, (int)ListMemberExcelEntity.Columns.STT + 1, rowEnd, (int)ListMemberExcelEntity.Columns.STT + 1)
                    //           .CopyTo(_objWorksheet.Range(rowData, (int)ListMemberExcelEntity.Columns.MemberName + 1, rowEnd, (int)ListMemberExcelEntity.Columns.MemberName + 1));
                    //_objWorksheet.Range(rowData, (int)ListMemberExcelEntity.Columns.STT + 1, rowEnd, (int)ListMemberExcelEntity.Columns.STT + 1)
                    //           .CopyTo(_objWorksheet.Range(rowData, (int)ListMemberExcelEntity.Columns.Gender + 1, rowEnd, (int)ListMemberExcelEntity.Columns.Gender + 1));
                    //_objWorksheet.Range(rowData, (int)ListMemberExcelEntity.Columns.STT + 1, rowEnd, (int)ListMemberExcelEntity.Columns.STT + 1)
                    //           .CopyTo(_objWorksheet.Range(rowData, (int)ListMemberExcelEntity.Columns.BirthDay + 1, rowEnd, (int)ListMemberExcelEntity.Columns.BirthDay + 1));
                    //_objWorksheet.Range(rowData, (int)ListMemberExcelEntity.Columns.STT + 1, rowEnd, (int)ListMemberExcelEntity.Columns.STT + 1)
                    //           .CopyTo(_objWorksheet.Range(rowData, (int)ListMemberExcelEntity.Columns.DeadDay + 1, rowEnd, (int)ListMemberExcelEntity.Columns.DeadDay + 1));
                    //_objWorksheet.Range(rowData, (int)ListMemberExcelEntity.Columns.STT + 1, rowEnd, (int)ListMemberExcelEntity.Columns.STT + 1)
                    //           .CopyTo(_objWorksheet.Range(rowData, (int)ListMemberExcelEntity.Columns.Father + 1, rowEnd, (int)ListMemberExcelEntity.Columns.Father + 1));
                    //_objWorksheet.Range(rowData, (int)ListMemberExcelEntity.Columns.STT + 1, rowEnd, (int)ListMemberExcelEntity.Columns.STT + 1)
                    //           .CopyTo(_objWorksheet.Range(rowData, (int)ListMemberExcelEntity.Columns.Mother + 1, rowEnd, (int)ListMemberExcelEntity.Columns.Mother + 1));
                }

                var lstIdx = lstIndxSpouse[cnt];
                int off = 0;

                foreach (var idx in lstIdx)
                {
                    int rEnd = rowData + off + idx;

                    if (rEnd > rowData + off)
                    {
                        MergeCells((int)ListMemberExcelEntity.Columns.Spouse + 1, rowData + off, rEnd);
                    }

                    off = (rEnd - rowData) + 1;
                };

                rowData = rowEnd + 1;
                cnt++;

                if (progressBar != null)
                {
                    progressBar.Percent = progressBar.fncCalculatePercent2(++progressBar.count, progressBar.total);
                }
            });
        }

        //private void MergeRowMemberAndSpouse(List<ExTMember> lstMember, List<int> lstIndex, List<List<int>> lstIndxSpouse, ProgressBarManager progressBar)
        //{
        //    int rowData = ListMemberExcelEntity.Row_DataStart;
        //    int cnt = 0;

        //    for (int i = 0; i < lstMember.Count; i++)
        //    {
        //        int rowEnd = rowData + lstIndex[cnt] - 1;
        //        if (rowEnd > rowData)
        //        {
        //            MergeCells(ListMemberExcelEntity.Col_STT, rowData, rowEnd);
        //            MergeCells(ListMemberExcelEntity.Col_LevelInFamily, rowData, rowEnd);
        //            MergeCells(ListMemberExcelEntity.Col_MemberName, rowData, rowEnd);
        //            MergeCells(ListMemberExcelEntity.Col_Gender, rowData, rowEnd);
        //            MergeCells(ListMemberExcelEntity.Col_Birthday, rowData, rowEnd);
        //            MergeCells(ListMemberExcelEntity.Col_DeadDay, rowData, rowEnd);
        //            MergeCells(ListMemberExcelEntity.Col_Father, rowData, rowEnd);
        //            MergeCells(ListMemberExcelEntity.Col_Mother, rowData, rowEnd);
        //        }
        //        var lstIdx = lstIndxSpouse[cnt];
        //        int off = 0;
        //        lstIdx.ForEach(idx =>
        //        {
        //            int rEnd = rowData + off + idx;
        //            if (rEnd > rowData + off)
        //            {
        //                MergeCells(ListMemberExcelEntity.Col_Spouse, rowData + off, rEnd);
        //            }
        //            off = (rEnd - rowData) + 1;
        //        });

        //        rowData = rowEnd + 1;
        //        cnt++;

        //        if (progressBar != null)
        //        {
        //            progressBar.Percent = progressBar.fncCalculatePercent2(++progressBar.count, progressBar.total);
        //        }
        //    };
        //}
        private Dictionary<string, object> GetMemberRelation2(Dictionary<string, ExTMember> dicTMember, Dictionary<string, TMemberRelation> tblTMemberRelation, ExTMember mainMember)
        {
            Dictionary<string, object> dicInfoMember = new Dictionary<string, object>();
            var lstParent = mainMember.ListPARENT;
            var lstSpouse = mainMember.ListSPOUSE;

            ExTMember tFather = new ExTMember();
            ExTMember tMother = new ExTMember();
            List<ExTMember> lstSpouseMember = new List<ExTMember>();

            lstParent = lstParent == null ? new List<string>() : lstParent;
            lstSpouse = lstSpouse == null ? new List<string>() : lstSpouse;

            if (lstParent.Count > 0)
            {
                foreach (string mem in lstParent)
                {
                    if (!dicTMember.ContainsKey(mem) || dicTMember[mem] == null) continue;
                    ExTMember member = dicTMember[mem];

                    member.RelTypeShow = tblTMemberRelation[mainMember.Id + mem]?.relType + "";

                    if (string.IsNullOrEmpty(member.Name))
                    {
                        continue;
                    }

                    tFather = member.RelTypeShow.Contains(Relation.PREFIX_DAD) ? member : tFather;
                    tMother = member.RelTypeShow.Contains(Relation.PREFIX_MOM) ? member : tMother;
                };
            }

            if (lstSpouse.Count > 0)
            {
                foreach (string mem in lstSpouse)
                {
                    if (!dicTMember.ContainsKey(mem) || dicTMember[mem] == null) continue;
                    var member = dicTMember[mem];
                    var relate = tblTMemberRelation[mainMember.Id + mem];
                    if (relate != null)
                    {
                        member.RelTypeShow = relate.relType;
                    }
                    else
                    {
                        member.RelTypeShow = "";
                    }
                    lstSpouseMember.Add(member);
                };
            }

            dicInfoMember.Add("Father", tFather);
            dicInfoMember.Add("Mother", tMother);
            dicInfoMember.Add("Spouse", lstSpouseMember);

            return dicInfoMember;
        }
        private List<ExTMember> GetChildren(Dictionary<string, ExTMember> dicTMember, Dictionary<string, TMemberRelation> tblTMemberRelation, ExTMember mainMember)
        {
            var lstChild = mainMember.ListCHILDREN;
            if (lstChild.Count == 0)
            {
                return new List<ExTMember>();
            }
            List<ExTMember> lstMemberRet = new List<ExTMember>();
            foreach (string mem in lstChild)
            {
                if (!dicTMember.ContainsKey(mem) || dicTMember[mem] == null) continue;
                var member = dicTMember[mem];
                var relate = tblTMemberRelation[mainMember.Id + mem];
                if (relate != null)
                {
                    member.RelTypeShow = relate.relType;
                }
                else
                {
                    member.RelTypeShow = "";
                }
                lstMemberRet.Add(member);
            };
            return lstMemberRet;
        }

        private int GetListInfoChildrenOfMember(Dictionary<string, ExTMember> dicAllMember, Dictionary<string, TMemberRelation> dicRelationTMember, ExTMember member, DataRow infoMember)
        {
            int numColum = Enum.GetNames(typeof(ListMemberExcelEntity.Columns)).Length;
            var lstChildOfMember = GetChildren(dicAllMember, dicRelationTMember, member);
            if (lstChildOfMember.Count > 0)
            {
                infoMember[(int)ListMemberExcelEntity.Columns.Child] = lstChildOfMember[0].Name;
            }
            tableMember.Rows.Add(infoMember);

            var lstInfoChild = lstChildOfMember.Where(x => !x.Id.Equals(lstChildOfMember[0].Id))
                                               .Select(x => x.Name).ToList();
            foreach (string child in lstInfoChild)
            {
                DataRow info = tableMember.NewRow();
                info[(int)ListMemberExcelEntity.Columns.Child] = child;
                tableMember.Rows.Add(info);
            }

            return lstInfoChild.Count + 1;
        }

        private void GetListInfoOfSpouse(Dictionary<string, ExTMember> dicAllMember, Dictionary<string, TMemberRelation> dicRelationTMember, ExTMember member, ExTMember spouse, DataRow infoMember, ref List<int> lstNumberChildOfSpouse, ref int index, ref List<string> listChildTemp)
        {
            int numColum = Enum.GetNames(typeof(ListMemberExcelEntity.Columns)).Length;

            infoMember[(int)ListMemberExcelEntity.Columns.Spouse] = spouse.Name;

            var lstChildOfSpouse1 = !spouse.InRootTree ? GetChildren(dicAllMember, dicRelationTMember, spouse) :
                                                         GetChildren(dicAllMember, dicRelationTMember, member);

            if (lstChildOfSpouse1.Count == 0)
            {
                tableMember.Rows.Add(infoMember);
                lstNumberChildOfSpouse.Add(0);
                index += 1;
            }
            else
            {
                lstNumberChildOfSpouse.Add(lstChildOfSpouse1.Count - 1);
                infoMember[(int)ListMemberExcelEntity.Columns.Child] = lstChildOfSpouse1[0].Name;
                tableMember.Rows.Add(infoMember);
                var lstInfoChild = lstChildOfSpouse1.Where(x => !x.Id.Equals(lstChildOfSpouse1[0].Id))
                                                                     .Select(x => x.Name)
                                                                     .ToList();
                foreach (string child in lstInfoChild)
                {
                    DataRow info = tableMember.NewRow();
                    info[(int)ListMemberExcelEntity.Columns.Child] = child;
                    tableMember.Rows.Add(info);
                }

                index += lstChildOfSpouse1.Count;
                listChildTemp.AddRange(spouse.ListCHILDREN);
            }

        }
        private Type GetValues<Type>(Dictionary<string, object> dictionary, string key)
        {
            if (dictionary.ContainsKey(key))
            {
                return (Type)dictionary[key];
            }
            Type defaultValue = default(Type);
            return defaultValue;
        }
    }
}
