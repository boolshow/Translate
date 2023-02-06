// See https://aka.ms/new-console-template for more information
using System.Net;
using Translate;

Console.WriteLine("Hello, World!");

GoogleTranslation GoogleTranslation=new();
MicrosoftTranslate MicrosoftTranslate=new();
BaiduTranslate baiduTranslate=new();

string txt = "Hello, World!";

//调用谷歌翻译样例
string Googletxt = await GoogleTranslation.GetGoogleTranslationAsync(txt);
Console.WriteLine($"Googletxt: {Googletxt}");

//调用微软翻译
//1.
string bingtxt=await MicrosoftTranslate.GetBingTranslateAsync(txt);
Console.WriteLine($"bingtxt: {bingtxt}");
//2
string MicrosoftTxt = await MicrosoftTranslate.GetMicrosoftTranslateAsync(txt);
Console.WriteLine($"MicrosoftTxt: {MicrosoftTxt}");

//调用百度翻译(百度自动识别目标语言和翻译接口是分开的)
string baidutxt = await baiduTranslate.GetBaiduTranslateAsync(txt);
Console.WriteLine($"baidutxt: {baidutxt}");
//百度识别当前字符串语言
string baidulang = await baiduTranslate.GetlangdetectAsync(txt);
Console.WriteLine($"百度识别当前字符串语言: {baidulang}");