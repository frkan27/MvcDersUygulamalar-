﻿/// <reference path="angular.js" />

var app = angular.module("myApp", ["dx"]);
var host = 'http://localhost:55599/';
app.controller("testCtrl", function ($scope) {

    $scope.dataGridOptions = {
        dataSource: customers,
        columns: ["CompanyName", "City", "State", "Phone", "Fax"],
        showBorders: true,
        searchPanel: {
            visible: true,
            width: 240,
            placeholder: "Ara..."
        }
    };

});

app.controller("customerCtrl", function ($scope, $http) {
    $scope.data = null;
    function init() {
        $http({
            url: host + 'api/customer/getall',
            method: 'GET'
        }).then(function (ev) {
            if (ev.data.success) {
                $scope.data = ev.data.data;
                loadGrid();
            }
        });
    }//js.DEVEXPRESS.COM DA VAR ÖRNEKLER...syncfusionda örnekler var kullanabilirsin ama biraz zor...

    function loadGrid() {
        $scope.dataGridOptions = {
            //dataSource: $scope.data,
               dataSource:{
store:{
type:"odata",
url:'/odata/CustomersOdata'
}
},
            selection:{
            mode:"multiple"//checkbox gelsin diye yaptık.  mode:single olsaydı tek seçim checkbox gelmezdi.
             },
            onSelectionChanged:function(selected){//seçtiğim kişiyi selected a atıyoruz.
            $scope.selected=selected.selectedRowsData;
             },
             "export":{//sağ üste export ekledik.export all data ile excell oluşturdu...layout a script ekledik.
              enabled:true,
             fileName:"customers"+parseInt(Math.random()*100000),
             allowExportSelectedData:true
             },
            columnChooser:{//sağ üstte column oluşuyor...
             enabled:true,
             allowSearch:true
},
            groupPanel:{//isme göre gruplamada kullanıyoruz...
visible:true
},
filterRow:{//arama işlemi yapabiliyoruz...
visible:true
},
headerFilter:{
visible:true//header da seçtiklerimiz geliyor.TARİH varsa kullanma ya da engelle!!! sıkıntı cıkarıyor... çünkü tarihde ortak bişey yok.ortak olanları ararsın isim,adres,şehir gibi...
},
            columns: [{
              dataField:"Id",
              caption:"Müşteri no",//ıd yi çekmek için çünkü seçim yapmak istiyoruz.
              visible:false
               },{
dataField:"Name",
groupIndex: 0
}, "Surname", "Phone",
 {
dataField:"Address",//denemek için addresi engelledik header filterla aranmasını...
allowHeaderFiltering:false//normalde tarih için engelleriz aynı gelme ihtimali az olan seyler için.
}, 
              { 
              dataField:"Balance",
              caption:"Balance",
              dataType:"number",
              format:"#,##0.## ₺"
            }],
            showBorders: true,
            paping:{pageSize:10},
            pager:{showPageSizeSelector:true,
            allowedPageSizes:[5,10,20],
            showInfo:true
            },
            searchPanel: {
                visible: true,
                width: 240,
                placeholder: "Ara..."
            },
summary:{//sağ altta toplam yazıyor...
totalItems:[{
column:"Balance",
summaryType:"sum",
valueFormat:"currency"
}],
groupItems:[{//isme göre grupluyoruz.o isimden kaç kişi varsa sayısını veriyor....
column:"Name",
summaryType:"count",
displayFormat:"Toplam:{0}"
}]
}
        };
    }

    init();
});

var customers=[{
    "ID": 1,
    "CompanyName": "Super Mart of the West",
    "Address": "702 SW 8th Street",
    "City": "Bentonville",
    "State": "Arkansas",
    "Zipcode": 72716,
    "Phone": "(800) 555-2797",
    "Fax": "(800) 555-2171",
    "Website": "http://www.nowebsitesupermart.com"
},{
    "ID": 2,
    "CompanyName": "Electronics Depot",
    "Address": "2455 Paces Ferry Road NW",
    "City": "Atlanta",
    "State": "Georgia",
    "Zipcode": 30339,
    "Phone": "(800) 595-3232",
    "Fax": "(800) 595-3231",
    "Website": "http://www.nowebsitedepot.com"
},{
    "ID": 3,
    "CompanyName": "K&S Music",
    "Address": "1000 Nicllet Mall",
    "City": "Minneapolis",
    "State": "Minnesota",
    "Zipcode": 55403,
    "Phone": "(612) 304-6073",
    "Fax": "(612) 304-6074",
    "Website": "http://www.nowebsitemusic.com"
},{
    "ID": 4,
    "CompanyName": "Tom's Club",
    "Address": "999 Lake Drive",
    "City": "Issaquah",
    "State": "Washington",
    "Zipcode": 98027,
    "Phone": "(800) 955-2292",
    "Fax": "(800) 955-2293",
    "Website": "http://www.nowebsitetomsclub.com"
},{
    "ID": 5,
    "CompanyName": "E-Mart",
    "Address": "3333 Beverly Rd",
    "City": "Hoffman Estates",
    "State": "Illinois",
    "Zipcode": 60179,
    "Phone": "(847) 286-2500",
    "Fax": "(847) 286-2501",
    "Website": "http://www.nowebsiteemart.com"
},{
    "ID": 6,
    "CompanyName": "Walters",
    "Address": "200 Wilmot Rd",
    "City": "Deerfield",
    "State": "Illinois",
    "Zipcode": 60015,
    "Phone": "(847) 940-2500",
    "Fax": "(847) 940-2501",
    "Website": "http://www.nowebsitewalters.com"
},{
    "ID": 7,
    "CompanyName": "StereoShack",
    "Address": "400 Commerce S",
    "City": "Fort Worth",
    "State": "Texas",
    "Zipcode": 76102,
    "Phone": "(817) 820-0741",
    "Fax": "(817) 820-0742",
    "Website": "http://www.nowebsiteshack.com"
},{
    "ID": 8,
    "CompanyName": "Circuit Town",
    "Address": "2200 Kensington Court",
    "City": "Oak Brook",
    "State": "Illinois",
    "Zipcode": 60523,
    "Phone": "(800) 955-2929",
    "Fax": "(800) 955-9392",
    "Website": "http://www.nowebsitecircuittown.com"
},{
    "ID": 9,
    "CompanyName": "Premier Buy",
    "Address": "7601 Penn Avenue South",
    "City": "Richfield",
    "State": "Minnesota",
    "Zipcode": 55423,
    "Phone": "(612) 291-1000",
    "Fax": "(612) 291-2001",
    "Website": "http://www.nowebsitepremierbuy.com"
},{
    "ID": 10,
    "CompanyName": "ElectrixMax",
    "Address": "263 Shuman Blvd",
    "City": "Naperville",
    "State": "Illinois",
    "Zipcode": 60563,
    "Phone": "(630) 438-7800",
    "Fax": "(630) 438-7801",
    "Website": "http://www.nowebsiteelectrixmax.com"
},{
    "ID": 11,
    "CompanyName": "Video Emporium",
    "Address": "1201 Elm Street",
    "City": "Dallas",
    "State": "Texas",
    "Zipcode": 75270,
    "Phone": "(214) 854-3000",
    "Fax": "(214) 854-3001",
    "Website": "http://www.nowebsitevideoemporium.com"
},{
    "ID": 12,
    "CompanyName": "Screen Shop",
    "Address": "1000 Lowes Blvd",
    "City": "Mooresville",
    "State": "North Carolina",
    "Zipcode": 28117,
    "Phone": "(800) 445-6937",
    "Fax": "(800) 445-6938",
    "Website": "http://www.nowebsitescreenshop.com"
}];