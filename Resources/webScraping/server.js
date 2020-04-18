var request = require("request");
var cheerio = require("cheerio");

console.log("processing...");

var url = "https://www.tic.com/index.html";

request(url, function(err, response, html) 
    {
        if(!err)
        {
            var $ = cheerio.load(html);
            var column1RelArray = [];
            var allItems = $("ul").children();
            //console.log(allItems);
            allItems.each(function(index)
            {
               // column1RelArray.push($("ul").children().eq(index).find("li").text());
                column1RelArray.push($(this).find('ul li').text());
            })         
            console.log("data i need",column1RelArray);
            
        }
    }
)
