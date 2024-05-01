use dotenv::dotenv;
use thirtyfour::prelude::*;

pub fn get_caps() -> Capabilities {
    dotenv().ok();

    let browser = std::env::var("BROWSER").unwrap();

    match browser.to_lowercase().as_str() {
        "chrome" => DesiredCapabilities::chrome().into(),
        "firefox" => DesiredCapabilities::firefox().into(),
        _ => panic!("Unrecognized browser"),
    }
}
