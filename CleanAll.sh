#!/bin/bash

find */obj -type d -exec rm -rv {} \; 2>/dev/null &
find */bin -type d -exec rm -rv {} \; 2>/dev/null &
find packages/* -type d -exec rm -rv {} \; 2>/dev/null &
rm -v Collector\ Agent/Input/*.txt &
wait
