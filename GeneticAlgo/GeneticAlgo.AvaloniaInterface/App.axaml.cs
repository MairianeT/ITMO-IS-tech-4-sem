using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using GeneticAlgo.AvaloniaInterface.ViewModels;
using GeneticAlgo.Shared;
using GeneticAlgo.Shared.Models;
using GeneticAlgo.Shared.Tools;
using Microsoft.Extensions.DependencyInjection;
using BenchmarkDotNet.Attributes;
using Point = GeneticAlgo.Shared.Models.Point;

namespace GeneticAlgo.AvaloniaInterface
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                const int maximum = 1;
                var collection = new ServiceCollection();
                collection.AddSingleton<MainWindowViewModel>();
                
                var data = new Configuration(1000, 0.05, 1,
                    new []{
                        new BarrierCircle(
                            new Point(0.33218833804130554, 0.14921106934547424), 
                            0.23818166553974152), 
                        new BarrierCircle(
                            new Point(0.9211785793304443, 0.21001200377941132), 
                            0.24298787117004395),
                        new BarrierCircle(
                            new Point(0, 1),
                            0.5
                            ),
                        new BarrierCircle(
                            new Point(0.75, 0.75),
                            0.15
                        )
                    }
                    );
                
                collection.AddSingleton<IExecutionContext>
                (_ => new DummyExecutionContext(
                    data.PointsCount, 
                    data.MaxValue, 
                    data.MaxLenght, 
                    data.Circles
                    ));
                
                collection.AddSingleton(new ExecutionConfiguration(TimeSpan.FromMilliseconds(1000), data.MaxValue, 0));

                var provider = collection.BuildServiceProvider();

                desktop.MainWindow = new MainWindow
                {
                    DataContext = provider.GetRequiredService<MainWindowViewModel>(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}