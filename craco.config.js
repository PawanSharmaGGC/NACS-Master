// craco.config.js
module.exports = {
  webpack: {
    configure: (config) => {
      // Modify the output filename for JavaScript files
      config.output.filename = 'static/js/[name].js';

      // Modify the output filename for CSS files
      config.plugins = config.plugins.map((plugin) => {
        if (plugin.constructor.name === 'MiniCssExtractPlugin') {
          plugin.options.filename = 'static/css/[name].css';
        }
        return plugin;
      });

      return config;
    }
  }
};