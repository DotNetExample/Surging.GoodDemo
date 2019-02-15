import react from 'react';
import styles from './ProductDetail.less'
import { InputNumber, Button } from 'antd'
export default class Detail extends react.Component {
    constructor(props){
        super(props)
        this.state={
            count:1,
            sumPrice:0
        }
    }
    componentDidMount(){
      
        this.props.GetForModify()
    }
    CountToggle=(c)=>{
        
        var cc=c;
        if(c>this.props.info.StockNum){
            cc=this.props.info.StockNum
        }
        if(!c||c<=0){
            cc=1;
        }
       var sum= this.props.info.Price*cc;
         this.setState({
             count:cc,
             sumPrice:sum
         })
    }

    CreateOrder=()=>{
          var goodsId=this.props.info.Id;
          var count=this.state.count;
          this.props.CreateOrder({GoodsId:goodsId,Count:count})
    }
    render() {
        const {info}=this.props;
        return (<div className={styles.detalWrap}>
            <div className={styles.detailImg}>
                <img src={info.CoverImgSrc}></img>
            </div>
            <div className={styles.detailContent}>
                <div className={styles.name}>
                  {info.Name}
                </div>
                <div className={styles.priceWrap}>
                    <div className={styles.itemLeft}>价格</div>
                    <div className={styles.price}>￥ {info.Price}</div>
                </div>

                <div className={styles.priceWrap}>
                    <div className={styles.itemLeft}>数量</div>
                    <div className={styles.price}><InputNumber onChange={
                       e=> this.CountToggle(e)
                    } value={this.state.count}/></div>
                    <div className={styles.stoc}>件 (库存{info.StockNum}件)</div>
                </div>
                <div className={styles.priceWrap}>
                <div className={styles.itemLeft}>总价</div>
                <div className={styles.price}>￥ {
                    this.state.count>1?this.state.sumPrice:info.Price

                }</div>
            </div>
                <div className={styles.btnWrap}>
                    <Button onClick={
                        e=>this.CreateOrder()
                    } className={styles.buy}>立即购买</Button>
                    <Button className={styles.car}>加入购物车</Button>
                </div>
            </div>
        </div>)
    }

}