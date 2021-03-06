#!/bin/bash
# stty -echo

BINARYDIR="./build_output/AutoTest.NET"
DEPLOYDIR="./ReleaseBinaries"
CASTLEDIR="./lib/Castle.Windsor"
VSADDINDIR="./addins/VisualStudio/FilesToDeploy"

if [ ! -d $DEPLOYDIR ]; then
{
	mkdir $DEPLOYDIR
}
else
{
	rm $DEPLOYDIR/*
}
fi

cp $BINARYDIR/AutoTest.Core.dll $DEPLOYDIR/AutoTest.Core.dll
cp $BINARYDIR/AutoTest.Console.exe $DEPLOYDIR/AutoTest.Console.exe
cp $BINARYDIR/AutoTest.WinForms.exe $DEPLOYDIR/AutoTest.WinForms.exe
cp $BINARYDIR/AutoTest.Console.exe.config $DEPLOYDIR/AutoTest.Console.exe.config
cp $BINARYDIR/AutoTest.config.template $DEPLOYDIR/AutoTest.config
cp ./README $DEPLOYDIR/README
cp ./LICENSE $DEPLOYDIR/AutoTest.License.txt
cp ./CHANGELOG $DEPLOYDIR/CHANGELOG

cp $BINARYDIR/Castle.Core.dll $DEPLOYDIR/Castle.Core.dll
cp $BINARYDIR/Castle.DynamicProxy2.dll $DEPLOYDIR/Castle.DynamicProxy2.dll
cp $BINARYDIR/Castle.Facilities.Logging.dll $DEPLOYDIR/Castle.Facilities.Logging.dll
cp $CASTLEDIR/Castle.license.txt $DEPLOYDIR/Castle.license.txt
cp $BINARYDIR/Castle.MicroKernel.dll $DEPLOYDIR/Castle.MicroKernel.dll
cp $BINARYDIR/Castle.Services.Logging.log4netIntegration.dll $DEPLOYDIR/Castle.Services.Logging.log4netIntegration.dll
cp $BINARYDIR/Castle.Windsor.dll $DEPLOYDIR/Castle.Windsor.dll

cp $BINARYDIR/log4net.dll $DEPLOYDIR/log4net.dll
cp $CASTLEDIR/log4net.license.txt $DEPLOYDIR/log4net.license.txt

cp $VSADDINDIR/AutoTest.VSAddin.AddIn $DEPLOYDIR/AutoTest.VSAddin.AddIn
cp $VSADDINDIR/AutoTest.VSAddin.dll $DEPLOYDIR/AutoTest.VSAddin.dll
cp $VSADDINDIR/How_to_install_VS_addin.txt $DEPLOYDIR/How_to_install_VS_addin.txt
