'use client';

import { useEffect, useRef } from 'react';
import Highcharts from 'highcharts';
import HighchartsReact from 'highcharts-react-official';
import { GenreCount } from '@/lib/api/booksApi';

interface BookChartProps {
  data: GenreCount[];
}

export function BookChart({ data }: BookChartProps) {
  const chartComponentRef = useRef<HighchartsReact.RefObject>(null);

  const options: Highcharts.Options = {
    chart: {
      type: 'pie',
      height: 300,
    },
    title: {
      text: 'Books by Genre',
    },
    tooltip: {
      pointFormat: '{series.name}: <b>{point.y} ({point.percentage:.1f}%)</b>',
    },
    accessibility: {
      point: {
        valueSuffix: '%',
      },
    },
    plotOptions: {
      pie: {
        allowPointSelect: true,
        cursor: 'pointer',
        dataLabels: {
          enabled: true,
          format: '<b>{point.name}</b>: {point.y}',
        },
      },
    },
    series: [
        {
            name: 'Books',
           // colorByPoint: true,
            type: 'pie',
            data: data.map((item) => ({
              name: item.genreName,
              y: item.bookCount,
            })),
          },
    ],
    credits: {
      enabled: false,
    },
  };

  return (
    <div className="highcharts-container">
      <HighchartsReact
        highcharts={Highcharts}
        options={options}
        ref={chartComponentRef}
      />
    </div>
  );
}