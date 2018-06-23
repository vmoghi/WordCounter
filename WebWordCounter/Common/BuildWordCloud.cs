using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using WebWordCounter.WebServices;
using WebWordCounter.Models;
namespace WebWordCounter.Common
{
     public interface IBuildWordCloud
        {
         IEnumerable<WordModel> GetListTopWords(string url);
        }
    public class BuildWordCloud:IBuildWordCloud
    {
       
        public IEnumerable<WordModel> GetListTopWords(string url)
        {
            if (string.IsNullOrEmpty(url.Trim()))
            {
                return null;
            }
            var onlyWords = new StringBuilder();
            var getPageResult = ProcessWebService.APIHttpRequest(url);
            if (getPageResult=="")
            {
                return null;
            }

            var docPage = new HtmlDocument();
            try
            {
                docPage.LoadHtml(getPageResult);
                var htmlNode = docPage.DocumentNode.Descendants().Where(
                        g => g.NodeType == HtmlNodeType.Text &&
                            g.ParentNode.Name != "script" &&
                            g.ParentNode.Name != "style");
                foreach (var nodeFound in htmlNode)
                {
                    var formatText = " " + nodeFound.InnerHtml.Trim();
                    onlyWords.Append(formatText);
                }
            }
            catch (Exception)
            {
                return null;
            }
            
            var removeExtraCharacters = RemoveSpecialCharacters(onlyWords.ToString()).ToLower();
            var query = removeExtraCharacters.ToString().Split(' ')
                       .GroupBy(c => c)
                       .Where(c => c.Key.Length > 3 )
                       .Select(o => new WordModel
                       {
                           Word = o.Key,
                           Counter=o.Count(),
                           Tag = GetTagClass(o.Count())
                       }).OrderByDescending(p=>p.Counter).Take(30);
            var randomOrderList=query.OrderBy(o => Guid.NewGuid());
            return randomOrderList;   
        }

        private static string RemoveSpecialCharacters(string text)
        {
            text=text.Replace("nbsp","");
            return Regex.Replace(text, "[^a-zA-Z ]", "", RegexOptions.Compiled);
        }

        private static string GetTagClass(int result)
        {
            if (result <= 1)
                return "tag1";
            if (result <= 4)
                return "tag2";
            if (result <= 8)
                return "tag3";
            if (result <= 12)
                return "tag4";
            if (result <= 18)
                return "tag5";
            if (result <= 30)
                return "tag6";
            return result <= 50 ? "tag7" : "tag8";
        }
    }
}