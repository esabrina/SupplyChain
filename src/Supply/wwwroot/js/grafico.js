function mostrar_grafico(v_ano, v_series, v_container, v_tipo) {
	Highcharts.chart(v_container, {

		title: {
			text: 'Movimentações - ' + v_tipo
		},

		subtitle: {
			text: v_tipo + ' mensal de produtos para o ano de ' + v_ano
		},

		yAxis: {
			title: {
				text: 'Quantidade'
			}
		},

		xAxis: {
			categories: ['Jan', 'Feb', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
			title: {
				text: 'Meses'
			}
		},

		legend: {
			layout: 'vertical',
			align: 'right',
			verticalAlign: 'middle'
		},

		plotOptions: {
			series: {
				label: {
					connectorAllowed: false
				}
			}
		},

		series: v_series,

		responsive: {
			rules: [{
				condition: {
					maxWidth: 500
				},
				chartOptions: {
					legend: {
						layout: 'horizontal',
						align: 'center',
						verticalAlign: 'bottom'
					}
				}
			}]
		}

	});
}

mostrar_grafico(v_ano, v_series_entrada, "container_entrada", "Entrada");
mostrar_grafico(v_ano, v_series_saida, "container_saida", "Saída");