$.package("ayatta.item", function () {
    this.bind = function (data) {
        var stock = 0;
        var skuResult = getSkus(data.skus);
        var array = lodash.values(data.skus);
        lodash.each(array, function (o) {
            stock += o.count;
        });

        $("#item-stock").text(stock);

        $(".item-sku-prop").each(function () {
            var d = $(this).data();
            var attrId = d.value;
            if (!skuResult[attrId]) {
                self.addClass("item-sku-prop-disabled");
            }
        }).click(function () {
            var self = $(this);
            if (self.hasClass("item-sku-prop-disabled")) {
                return false;
            } else {
                self.toggleClass("item-sku-prop-selected").siblings().removeClass("item-sku-prop-selected");
            }
            var selectedObjs = $(".item-sku-prop-selected");
            if (selectedObjs.length) {
                //获得组合key价格
                var selectedIds = [];
                selectedObjs.each(function () {
                    var d = $(this).data();
                    var value = d.value;
                    selectedIds.push(value);
                });
                selectedIds.sort(function (a, b) {
                    return parseInt(a) - parseInt(b);
                });
                var len = selectedIds.length;
                var count = skuResult[selectedIds.join(";")].count;
                var prices = skuResult[selectedIds.join(";")].prices;
                var maxPrice = Math.max.apply(Math, prices);
                var minPrice = Math.min.apply(Math, prices);

                $("#item-stock").text(count);
                $("#item-price").text(maxPrice > minPrice ? minPrice + "-" + maxPrice : maxPrice);
                //用已选中的节点验证待测试节点
                $(".item-sku-prop").not(selectedObjs).not(self).each(function () {
                    var d = $(this).data();
                    var value = d.value;
                    var siblingsSelectedObj = $(this).siblings(".item-sku-prop-selected");
                    var testAttrIds = []; //从选中节点中去掉选中的兄弟节点
                    if (siblingsSelectedObj.length) {
                        var siblingsSelectedObjId = siblingsSelectedObj.data().value;
                        for (var i = 0; i < len; i++) {
                            (selectedIds[i] !== siblingsSelectedObjId) && testAttrIds.push(selectedIds[i]);
                        }
                    } else {
                        testAttrIds = selectedIds.concat();
                    }
                    testAttrIds = testAttrIds.concat(value);
                    testAttrIds.sort(function (a, b) {
                        return parseInt(a) - parseInt(b);
                    });
                    if (!skuResult[testAttrIds.join(";")] || skuResult[testAttrIds.join(";")].count < 1) {
                        $(this).removeClass("item-sku-prop-selected").addClass("item-sku-prop-disabled");
                    } else {
                        $(this).removeClass("item-sku-prop-disabled");
                    }
                });
            } else {
                //设置默认价格
                $("#item-price").text("--");
                //设置属性状态
                $(".item-sku-prop").each(function () {
                    var value = $(this).data().value;
                    skuResult[value] ? $(this).removeClass("item-sku-prop-disabled") : $(this).removeClass("item-sku-prop-selected").addClass("item-sku-prop-disabled");
                });
            }
            return false;
        });

        //添加到购物车
        $(".item-action-add a").click(function () {

            var stock = data.stock;
            var itemId = data.id;
            var skuId = 0;
            /*
			if (Ayatta.Identity.IsAuthenticated == false) {
				Ayatta.Passport.Fun.login();
				return false;
			}
			*/
            if (data.propCount > 0) {
                var selectedProp = $(".item-sku-prop-selected");
                if (selectedProp.length !== i) {
                    $(".item-sku").addClass("item-sku-attention");
                    return false;
                } else {
                    $(".item-sku").removeClass("item-sku-attention");
                }

                var values = [];
                lodash.each(selectedProp, function (o) {
                    var d = $(o).data();
                    var value = d.value;
                    values.push(value);
                });
                var sku = data[values.join(";")];
                if (sku == null) {
                    alert("sku错误");
                    return false;
                }
                stock = sku.count;
                skuId = sku.id;
            }
            var quantity = parseInt($("#item-quantity").val()) || 1;

            if (quantity > stock) {
                alert("数量不对");
                return false;
            }

            var url = $.format('{0}/add/{1}?skuId={2}&qty={3}&callback=?', 'http://cart.ayatta.com/cart', itemId, skuId, quantity);
            $.getJSON(url, function (result) {
                console.log(result);
                if (result.Status === true) {
                    alert("0");
                    //Sk.Cart.RenderMiniCart();
                }
            });
            return false;
        });


        var ez = $("#zoom").elevateZoom({ gallery: ['gallery-zoom-thumb'], cursor: "move", zoomWindowWidth: 440, zoomWindowHeight: 440, zoomWindowOffetx: 20 });

        $(".item-sku-prop-img").click(function () {
            if (!$(this).parent().hasClass("item-sku-prop-disabled")) {
                var img = $(this).data("image");
                var temp = ez.data("elevateZoom");

                temp.elem.src = img;
                temp.options.zoomEnabled = false;
                $(".zoom-active").removeClass("zoom-active");
            }
        });
    };

    function getSkus(data) {
        var skuResult = {};
        //初始化得到结果集
        var i, j, skuKeys = lodash.keys(data);
        //把组合的key放入结果集SKUResult
        function addToSkuResult(key, sku) {
            if (skuResult[key]) {//SKU信息key属性·
                skuResult[key].count += sku.count;
                skuResult[key].prices.push(sku.price);
            } else {
                skuResult[key] = {
                    count: sku.count,
                    prices: [sku.price]
                };
            }
        }

        //对一条SKU信息进行拆分组合
        function combineSku(skuKeyAttrs, cnum, sku) {
            var len = skuKeyAttrs.length;
            for (var i = 0; i < len; i++) {
                var key = skuKeyAttrs[i];
                for (var j = i + 1; j < len; j++) {
                    if (j + cnum <= len) {
                        var tempArr = skuKeyAttrs.slice(j, j + cnum);    //安装组合个数获得属性值·
                        var genKey = key + ";" + tempArr.join(";");    //得到一个组合key
                        addToSkuResult(genKey, sku);
                    }
                }
            }
        }

        for (i = 0; i < skuKeys.length; i++) {
            var skuKey = skuKeys[i]; //一条SKU信息key
            var sku = data[skuKey];    //一条SKU信息value
            var skuKeyAttrs = skuKey.split(";"); //SKU信息key属性值数组
            var len = skuKeyAttrs.length;

            //对每个SKU信息key属性值进行拆分组合
            for (j = 0; j < len; j++) {
                //单个属性值作为key直接放入SKUResult
                addToSkuResult(skuKeyAttrs[j], sku);
                //对本组SKU信息key属性进行组合，组合个数为j
                (j > 0 && j < len - 1) && combineSku(skuKeyAttrs, j, sku);
            }

            //结果集接放入SKUResult
            skuResult[skuKey] = {
                count: sku.count,
                prices: [sku.price]
            };
        }
        return skuResult;
    }
});

