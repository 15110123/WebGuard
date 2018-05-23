using WebGuard.Utils;
using static System.Console;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebGuard.Utils.Tests
{
    [TestClass]
    public class StringDistanceUtilsTests
    {
        [TestMethod]
        public void DistanceTest()
        {
            var res = StringDistanceUtils.Distance("yNostamps,envelopes,orcheckstowritegiveyoumoretimetospendonthethingsyouenjoy.RealEstateFinancingFast.Simple.Professional.Whetheryouarepreparingtobuy,build,purchaseland,orconstructnewspace,letAltoroMutual\'spremierrealestatelendershelpwithfinancing.Asaregionalleader,weknowthemarket,weunderstandthebusiness,andwehavethetrackrecordtoproveit   BusinessCreditCardsYou\'realwayslookingforwaystoimproveyourcompany\'sbottomline.Youwanttobeinformed,improveefficiencyandcontrolexpenses.Now,youcandoitall-withabusinesscreditcardaccountfromAltoroMutual.RetirementSolutionsRetaininggoodemployeesisatoughtask.SeehowAltoroMutualcanassistyouinaccomplishingthisfeatthrougheffectiveRetirementSolutions.  PrivacyandSecurityThe2000employeesofAltoroMutualarededicatedtoprotectingyourprivacyandsecurity.Wepledgetoprovideyouwiththeinformationandresourcesthatyouneedtohelpsecureyourinformationandkeepitconfidential.Thisisourpromise.Winan8GBiPodNanoCompletingthisshortsurveywillenteryouinadrawfor1of50iPodNanos.Welookforwardtohearingyourimportantfeedback.PrivacyPolicy  |  SecurityStatement  |  c2018AltoroMutual,Inc.TheAltoroMutualwebsiteispublishedbyWatchfire,Inc.forthesolepurposeofdemonstratingtheeffectivenessofWatchfireproductsindetectingwebapplicationvulnerabilitiesandwebsitedefects.Thissiteisnotarealbankingsite.Similarities,ifany,tothirdpartyproductsandorwebsitesarepurelycoincidental.Thissiteisprovided\"asis\"withoutwarrantyofanykind,eitherexpressorimplied.Watchfiredoesnotassumeanyriskinrelationtoyouruseofthiswebsite.ForadditionalTermsofUse,pleasegoto:www.watchfire.comstatementsterms.aspx.Copyrightc2018,WatchfireCorporation,Allrightsreserved.:testfire.netimageslogo.:testfire.netimagesheader_pic.:testfire.netimagespf_lock.:testfire.netimageshome1.:testfire.netimageshome2.:testfire.netimageshome3.", ":testfire.netwelcome.").Result;
            WriteLine(res);
            Assert.AreEqual(res, 2);
        }

        [TestMethod()]
        public void DistanceEqualPercentsTest()
        {
            var res = StringDistanceUtils.DistanceEqualPercents("yNostamps,envelopes,orcheckstowritegiveyoumoretimetospendonthethingsyouenjoy.RealEstateFinancingFast.Simple.Professional.Whetheryouarepreparingtobuy,build,purchaseland,orconstructnewspace,letAltoroMutual\'spremierrealestatelendershelpwithfinancing.Asaregionalleader,weknowthemarket,weunderstandthebusiness,andwehavethetrackrecordtoproveit   BusinessCreditCardsYou\'realwayslookingforwaystoimproveyourcompany\'sbottomline.Youwanttobeinformed,improveefficiencyandcontrolexpenses.Now,youcandoitall-withabusinesscreditcardaccountfromAltoroMutual.RetirementSolutionsRetaininggoodemployeesisatoughtask.SeehowAltoroMutualcanassistyouinaccomplishingthisfeatthrougheffectiveRetirementSolutions.  PrivacyandSecurityThe2000employeesofAltoroMutualarededicatedtoprotectingyourprivacyandsecurity.Wepledgetoprovideyouwiththeinformationandresourcesthatyouneedtohelpsecureyourinformationandkeepitconfidential.Thisisourpromise.Winan8GBiPodNanoCompletingthisshortsurveywillenteryouinadrawfor1of50iPodNanos.Welookforwardtohearingyourimportantfeedback.PrivacyPolicy  |  SecurityStatement  |  c2018AltoroMutual,Inc.TheAltoroMutualwebsiteispublishedbyWatchfire,Inc.forthesolepurposeofdemonstratingtheeffectivenessofWatchfireproductsindetectingwebapplicationvulnerabilitiesandwebsitedefects.Thissiteisnotarealbankingsite.Similarities,ifany,tothirdpartyproductsandorwebsitesarepurelycoincidental.Thissiteisprovided\"asis\"withoutwarrantyofanykind,eitherexpressorimplied.Watchfiredoesnotassumeanyriskinrelationtoyouruseofthiswebsite.ForadditionalTermsofUse,pleasegoto:www.watchfire.comstatementsterms.aspx.Copyrightc2018,WatchfireCorporation,Allrightsreserved.:testfire.netimageslogo.:testfire.netimagesheader_pic.:testfire.netimagespf_lock.:testfire.netimageshome1.:testfire.netimageshome2.:testfire.netimageshome3.", ":testfire.netwelcome.", true).Result;
            WriteLine($"Percent: {res}");
        }
    }
}